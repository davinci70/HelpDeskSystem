using HelpDesk.Application.Common.Interfaces;

namespace HelpDesk.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me(
    [FromServices] ICurrentUserService currentUser)
    {
        return Ok(new
        {
            currentUser.UserId,
            currentUser.Email,
            currentUser.FullName,
            currentUser.Role
        });
    }
}