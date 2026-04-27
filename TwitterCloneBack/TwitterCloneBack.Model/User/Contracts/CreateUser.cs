using System.ComponentModel.DataAnnotations;

namespace TwitterCloneBack.Model.User.Contracts;

public class CreateUser
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string DisplayUsername { get; set; } = null!;

    [Required] public string Password { get; set; } = "";

    public string? Bio { get; set; }
}