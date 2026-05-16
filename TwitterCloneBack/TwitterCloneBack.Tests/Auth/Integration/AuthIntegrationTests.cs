using System.Net;
  using System.Net.Http.Json;
  using System.Text;
  using System.Text.Json;
  using Microsoft.Extensions.DependencyInjection;
  using TwitterCloneBack.Dal;
  using TwitterCloneBack.Dal.User.Dao;
  using TwitterCloneBack.Login.Contracts;
  using TwitterCloneBack.User.Contracts;

  namespace TwitterCloneBack.Tests.Auth.Integration;

  public class AuthIntegrationTests : IClassFixture<TwitterCloneWebApplicationFactory<Program>>
  {
      private const string ApiPrefix = "/api/v1/auth";

      private readonly TwitterCloneWebApplicationFactory<Program> _factory;
      private readonly HttpClient _http;

      public AuthIntegrationTests(TwitterCloneWebApplicationFactory<Program> factory)
      {
          _factory = factory;
          _http = factory.CreateClient();
      }
      [Fact]
      public async Task Login_WhenCredentialsAreCorrect_ReturnsToken()
      {
          await SeedUserAsync(
              id: 1,
              email: "test@test.com",
              username: "test",
              password: "Password123!");

          var response = await _http.PostAsJsonAsync(
              $"{ApiPrefix}/login",
              new LoginUser("test@test.com", "Password123!"));

          var body = await response.Content.ReadAsStringAsync();

          Assert.True(response.StatusCode == HttpStatusCode.OK, body);

          using var json = JsonDocument.Parse(body);

          Assert.Equal(1, json.RootElement.GetProperty("userId").GetInt32());
          Assert.False(string.IsNullOrWhiteSpace(
              json.RootElement.GetProperty("token").GetString()));
      }

      [Fact]
      public async Task Login_WhenPasswordIsWrong_ReturnsUnauthorized()
      {
          await SeedUserAsync(
              id: 1,
              email: "test@test.com",
              username: "test",
              password: "Password123!");

          var response = await _http.PostAsJsonAsync(
              $"{ApiPrefix}/login",
              new LoginUser("test@test.com", "WrongPassword123!"));

          Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
      }

      [Fact]
      public async Task Register_WhenUserIsNew_ReturnsCreatedUser()
      {
          await ClearUsersAsync();

          var response = await _http.PostAsJsonAsync(
              ApiPrefix,
              new RegisterUser(
                  "newuser",
                  "New User",
                  "new@test.com",
                  "Password123!",
                  "bio"));

          var body = await response.Content.ReadAsStringAsync();

          Assert.True(response.StatusCode == HttpStatusCode.OK, body);

          var user = await response.Content.ReadFromJsonAsync<GetUser>();

          Assert.NotNull(user);
          Assert.Equal("newuser", user.Username);
      }

      [Theory]
      [MemberData(nameof(InvalidRegisterUsers))]
      public async Task Register_WhenRequestIsInvalid_ReturnsBadRequest(
          RegisterUser registerUser)
      {
          await ClearUsersAsync();

          var response = await _http.PostAsJsonAsync(ApiPrefix, registerUser);

          Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
      }

      [Fact]
      public async Task Register_WhenBodyIsMissing_ReturnsBadRequest()
      {
          await ClearUsersAsync();

          var response =
              await _http.PostAsJsonAsync<RegisterUser?>(ApiPrefix, null);

          Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
      }

      [Fact]
      public async Task Register_WhenEmailAlreadyExists_ReturnsBadRequest()
      {
          await SeedUserAsync(
              id: 1,
              email: "existing@test.com",
              username: "existing",
              password: "Password123!");

          var response = await _http.PostAsJsonAsync(
              ApiPrefix,
              CreateRegisterUser(
                  username: "another",
                  email: "existing@test.com"));

          Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
      }

      [Fact]
      public async Task Register_WhenUsernameAlreadyExists_ReturnsBadRequest()
      {
          await SeedUserAsync(
              id: 1,
              email: "existing@test.com",
              username: "existing",
              password: "Password123!");

          var response = await _http.PostAsJsonAsync(
              ApiPrefix,
              CreateRegisterUser(
                  username: "existing",
                  email: "another@test.com"));

          Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
      }

      [Fact]
      public async Task Register_WhenBioIsNull_ReturnsCreatedUser()
      {
          await ClearUsersAsync();

          var response = await _http.PostAsJsonAsync(
              ApiPrefix,
              CreateRegisterUser(bio: null));

          var body = await response.Content.ReadAsStringAsync();

          Assert.True(response.StatusCode == HttpStatusCode.OK, body);

          var user = await response.Content.ReadFromJsonAsync<GetUser>();

          Assert.NotNull(user);
          Assert.Null(user.Bio);
      }

      [Fact]
      public async Task Register_WhenRequestIsValid_SavesHashedPassword()
      {
          const string password = "Password123!";

          await ClearUsersAsync();

          var response = await _http.PostAsJsonAsync(
              ApiPrefix,
              CreateRegisterUser(password: password));

          var body = await response.Content.ReadAsStringAsync();

          Assert.True(response.StatusCode == HttpStatusCode.OK, body);

          using var scope = _factory.Services.CreateScope();
          var db = scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();
          var user = db.Users.Single();
          var passwordHash = Encoding.UTF8.GetString(user.PasswordHash);

          Assert.NotEqual(password, passwordHash);
          Assert.True(BCrypt.Net.BCrypt.Verify(password, passwordHash));
      }

      public static TheoryData<RegisterUser> InvalidRegisterUsers()
      {
          return new TheoryData<RegisterUser>
          {
              CreateRegisterUser(username: null!),
              CreateRegisterUser(username: "ab"),
              CreateRegisterUser(username: new string('a', 31)),
              CreateRegisterUser(displayUsername: null!),
              CreateRegisterUser(displayUsername: "ab"),
              CreateRegisterUser(displayUsername: new string('a', 31)),
              CreateRegisterUser(email: null!),
              CreateRegisterUser(email: "not-email"),
              CreateRegisterUser(password: null!),
              CreateRegisterUser(password: "1234567"),
              CreateRegisterUser(bio: new string('a', 257))
          };
      }

      private static RegisterUser CreateRegisterUser(
          string username = "newuser",
          string displayUsername = "New User",
          string email = "new@test.com",
          string password = "Password123!",
          string? bio = "bio")
      {
          return new RegisterUser(
              username,
              displayUsername,
              email,
              password,
              bio!);
      }

      private async Task SeedUserAsync(
          int id,
          string email,
          string username,
          string password)
      {
          await ClearUsersAsync();

          using var scope = _factory.Services.CreateScope();
          var db = scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

          db.Users.Add(new UserDao
          {
              Id = id,
              Username = username,
              Email = email,
              DisplayUsername = username,
              Bio = "test bio",
              PasswordHash = Encoding.UTF8.GetBytes(
                  BCrypt.Net.BCrypt.HashPassword(password)),
              CreatedAt = DateTime.UtcNow
          });

          await db.SaveChangesAsync();
      }

      private async Task ClearUsersAsync()
      {
          using var scope = _factory.Services.CreateScope();
          var db = scope.ServiceProvider.GetRequiredService<TwitterCloneContext>();

          db.Users.RemoveRange(db.Users);
          await db.SaveChangesAsync();
      }
  }
