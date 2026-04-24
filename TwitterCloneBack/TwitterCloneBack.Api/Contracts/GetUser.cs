namespace TwitterCloneBack.Contracts;

public class GetUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string DisplayUsername { get; set; } = null!;

    public string? Bio { get; set; }

    public DateTime CreatedAt { get; set; }
}