namespace TwitterCloneBack.Login.Contracts;

public record RegisterUser(
    string Username,
    string DisplayUsername,
    string Email,
    string UnhashedPassword,
    string Bio
);