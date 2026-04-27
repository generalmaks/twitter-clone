namespace TwitterCloneBack.Model.User.Model;

public class UserDto
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string DisplayUsername { get; set; } = null!;

    public string? Bio { get; set; }

    public byte[] PasswordHash { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
