namespace lojatcc.authentication.api.Domain.Commands.V1.GetLogin;

public class GetLoginCommandResponse
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Role { get; set; }

    public Guid Token { get; set; }
}