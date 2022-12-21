using lojatcc.authentication.api.Domain.Commands.V1.Login;
using lojatcc.authentication.api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using lojatcc.authentication.api.Domain.SignIn;
using lojatcc.authentication.api.Services.Entities;
using lojatcc.authentication.api.Domain.Commands.V1.GetLogin;

namespace lojatcc.authentication.api.Controllers.V1;

[ApiController]
[Route("authentication/api/v1")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly UserContext _Context;

    public AuthenticationController(ILogger<AuthenticationController> logger, UserContext context)
    {
        _logger = logger;
        _Context = context;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginCommand request)
    {
        _logger.LogInformation($"Login process starting... ");

        try
        {
            var user = await _Context.Users.FirstOrDefaultAsync(x => x.Username.Equals(request.Username) && x.Password.Equals(request.Password));
            if(user == null)
                return Unauthorized(new 
                { 
                    Messages = new[] { "Username or Password is Wrong!" }
                });
            
            user.Token = Guid.NewGuid();

            _Context.Update(user);
            _ =_Context.SaveChangesAsync();

            return Ok(new LoginCommandResponse 
            {
                Token = user.Token
            });
        } 
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpGet("login")]
    public async Task<IActionResult> GetLoginAsync([FromHeader(Name = "Authorization")] Guid authorization)
    {
        _logger.LogInformation($"Login process starting... ");

        try
        {
            var user = await _Context.Users.FirstOrDefaultAsync(x => x.Token.Equals(authorization));
            if(user == null)
                return Unauthorized(new 
                { 
                    Messages = new[] { "You need to log in to access!" }
                });

            return Ok(new GetLoginCommandResponse 
            {
                Id = user.Id,
                Role = user.Role,
                Username = user.Username,
                Token = user.Token
            });
        } 
        catch (Exception e)
        {
            throw;
        }
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInCommand request)
    {
        _logger.LogInformation($"SignIn process starting... ");

        try
        {
            var user = await _Context.Users.FirstOrDefaultAsync(x => x.Username.Equals(request.Username));
            if(user != null)
                return Conflict(new 
                { 
                    Messages = new[] { "This username is already in use!" }
                });
            
            
            _Context.Users.Add(new User
            {
                Username = request.Username,
                Password = request.Password,
                Role = "Default",
                Token = Guid.Empty
            });
            await _Context.SaveChangesAsync();

            return Created("", null);
        } 
        catch (Exception e)
        {
            throw;
        }
    }
}