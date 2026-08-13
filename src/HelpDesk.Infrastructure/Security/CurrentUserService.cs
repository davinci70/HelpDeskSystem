namespace HelpDesk.Infrastructure.Security;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated == true;

    public int? UserId
    {
        get
        {
            var value = User?.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User?.FindFirstValue("sub");

            return int.TryParse(value, out var id)
                ? id
                : null;
        }
    }

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email);

    public string? FullName =>
        User?.FindFirstValue(ClaimTypes.Name);

    public string? Role =>
        User?.FindFirstValue(ClaimTypes.Role);
}
