namespace HelpDesk.Application.Common.Errors;

public sealed record Error(string Code, string Description, int? StatusCode = null)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
