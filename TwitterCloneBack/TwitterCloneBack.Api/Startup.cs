using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TwitterCloneBack.Dal;
using TwitterCloneBack.Dal.Like.Repository;
using TwitterCloneBack.Dal.Post.Repository;
using TwitterCloneBack.Dal.User.Repository;
using TwitterCloneBack.Login;
using TwitterCloneBack.Model.Like.Interfaces;
using TwitterCloneBack.Model.Post.Interfaces;
using TwitterCloneBack.Model.User.Interfaces;
using TwitterCloneBack.Orchestrator.Like.Orchestrator;
using TwitterCloneBack.Orchestrator.Post.Orchestrator;
using TwitterCloneBack.Orchestrator.User.Orchestrator;
using TwitterCloneBack.User.Controller;

namespace TwitterCloneBack;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureAuthentication(services);

        services.AddAuthorization();
        services
            .AddControllers()
            .AddApplicationPart(typeof(UserController).Assembly);
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngular", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        services.AddOpenApi();
        ConfigureDb(services);

        services.AddAutoMapper(_ => { },
            AppDomain.CurrentDomain.GetAssemblies());

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserOrchestrator, UserOrchestrator>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostOrchestrator, PostOrchestrator>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<ILikeOrchestrator, LikeOrchestrator>();
        services.AddSingleton<JwtTokenGenerator>();

        services.AddEndpointsApiExplorer();
        ConfigureSwagger(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseCors("AllowAngular");
        app.UseMiddleware<GlobalExceptionMiddleware>();
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            if (env.IsDevelopment())
                endpoints.MapOpenApi();

            endpoints.MapControllers();
        });
    }

    protected virtual void ConfigureDb(IServiceCollection services)
    {
        services.AddDbContext<TwitterCloneContext>(options =>
            options.UseSqlServer(
                _configuration.GetConnectionString("SqlServer")));
    }

    private void ConfigureAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var key =
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = _configuration["Jwt:Issuer"],
                        ValidAudience = _configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
            });
    }

    private static void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.ApiKey,
                Description = "Jwt authorization",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Flows = null,
                OpenIdConnectUrl = null,
                Extensions = null,
                UnresolvedReference = false,
                Reference = null
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference =
                            new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                    },
                    []
                }
            });
        });
    }
}
