namespace TwitterCloneBack.Model.Contracts;

public class UpdateUser
{
    public int Id { get; set; }

    public string? Username { get; set; } = null!;

    public string? DisplayUsername { get; set; } = null!;

    public string? Bio { get; set; }

    public string? Password { get; set; } = null!;
}