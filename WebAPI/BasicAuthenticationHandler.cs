using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Services.Abstractions;

namespace WebAPI;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string error = "Unauthorized";
    private readonly IUserService _userService;
    
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, IUserService userService):base(options, logger, encoder, clock)
    {
        _userService = userService;
    }

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if(!Request.Headers.ContainsKey("Authorization") || !AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out var value))
            return AuthenticateResult.Fail("'Authorization' header not found.");
        if(value.Scheme != "Basic")
            return AuthenticateResult.Fail("'Basic' scheme is only supported");
        
        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(value.Parameter));
        if (string.IsNullOrEmpty(credentials)) 
            return AuthenticateResult.Fail(error);
        var inputs = credentials.Split(':');
        if(!await _userService.AuthenticateAsync(inputs[0], inputs[1]))
            return AuthenticateResult.Fail(error);
        return CreateSuccess(inputs[0]);
    }

    private AuthenticateResult CreateSuccess(string login)
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, login) },
            Scheme.Name));
        return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
    }

    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.Headers.Add("WWW-Authenticate", "Basic");
        return base.HandleChallengeAsync(properties);
    }
}