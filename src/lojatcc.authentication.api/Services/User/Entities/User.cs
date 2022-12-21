using System.ComponentModel.DataAnnotations;

namespace lojatcc.authentication.api.Services.Entities;

public class User
{
    [Key]
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Role { get; set; }

    public string Password { get; set; }

    public Guid Token { get; set; }
}