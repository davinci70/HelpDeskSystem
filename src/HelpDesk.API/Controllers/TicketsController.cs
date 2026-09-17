namespace HelpDesk.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TicketsController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] int Id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTicketByIdQuery(Id), cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] GetTicketsQuery request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request.Adapt<CreateTicketCommand>(), cancellationToken);
        return result.IsSuccess ? CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value) : result.ToProblem();
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateTicketRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request.Adapt<UpdateTicketCommand>() with { Id = Id }, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{Id}/assign")]
    public async Task<IActionResult> Assign([FromRoute] int Id, [FromBody] AssignTicketRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request.Adapt<AssignTicketCommand>() with { TicketId = Id }, cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{Id}/startProgress")]
    public async Task<IActionResult> StartProgress([FromRoute] int Id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new StartTicketProcessCommand(Id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    
    [HttpPost("{Id}/resolve")]
    public async Task<IActionResult> Resolve([FromRoute] int Id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ResolveTicketCommand(Id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    
    [HttpPost("{Id}/close")]
    public async Task<IActionResult> Close([FromRoute] int Id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new CloseTicketCommand(Id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    
    [HttpPost("{Id}/reopen")]
    public async Task<IActionResult> Reopen([FromRoute] int Id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new ReopenTicketCommand(Id), cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpPost("{Id}/comments")]
    public async Task<IActionResult> AddComment([FromRoute] int Id, [FromBody] CreateTicketCommentRequest request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request.Adapt<CreateTicketCommentCommand>() with { TicketId = Id }, cancellationToken);
        return result.IsSuccess ? StatusCode(StatusCodes.Status201Created, result.Value) : result.ToProblem();
    }
}
