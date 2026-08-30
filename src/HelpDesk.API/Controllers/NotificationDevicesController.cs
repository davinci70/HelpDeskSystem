using HelpDesk.Application.NotificationDevices.Commands.RegisterNotificationDevice;

namespace HelpDesk.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationDevicesController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterNotificationDeviceCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }   
}
