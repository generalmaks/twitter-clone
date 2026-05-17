using System.ComponentModel.DataAnnotations;

namespace TwitterCloneBack.Login.Contracts;

public record RegisterUser(
    [Required]
    [StringLength(30, MinimumLength = 3)]
    string Username,
    [Required]
    [StringLength(30, MinimumLength = 3)]
    string DisplayUsername,
    [Required] [EmailAddress]
    string Email,
    [Required] [MinLength(8)]
    string UnhashedPassword,
    [MaxLength(256)] string? Bio
);