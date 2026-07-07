namespace HelpDesk.Application.Errors;

public static class AgentErrors
{
    public static Error AgentNotFound =
        new("Agents.NotFound", "The specified agent does not exist.", StatusCodes.Status404NotFound);
    
    public static Error AgentNotActive =
        new("Agents.NotActive", "The specified agent is inactive.", StatusCodes.Status400BadRequest);
}
