namespace HelpDesk.Application.Errors;


public static class TicketErrors
{
    public static Error CategoryNotFound =
        new( "Categories.NotFound", "The specified category does not exist.", StatusCodes.Status404NotFound);
    
    public static Error TicketNotFound =
        new("Tickets.NotFound", "The ticket was not found", StatusCodes.Status404NotFound);

    public static readonly Error CannotStart = 
        new("Tickets.CannotStart", "Only open tickets can be started.", StatusCodes.Status409Conflict);

    public static readonly Error NotAssigned = 
        new( "Tickets.NotAssigned", "The ticket must be assigned before it can be started.",StatusCodes.Status409Conflict);

    public static readonly Error CannotResolve =
        new( "Tickets.CannotResolve", "Only tickets in progress can be resolved.", StatusCodes.Status409Conflict);

    public static readonly Error CannotClose =
        new("Tickets.CannotClose", "Only resolved tickets can be closed.", StatusCodes.Status409Conflict);
    
    public static readonly Error CannotReopen =
        new("Tickets.CannotReopen", "Only closed tickets can be reopened.", StatusCodes.Status409Conflict);
    
    public static readonly Error UserIsNotAnAgent =
        new("Tickets.UserIsNotAnAgent", "The selected user is not an agent.", StatusCodes.Status409Conflict);
}