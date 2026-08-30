namespace HelpDesk.Application.NotificationDevices.Commands.RegisterNotificationDevice;

public sealed class RegisterNotificationDeviceCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    : IRequestHandler<RegisterNotificationDeviceCommand, Result>
{
    private readonly IApplicationDbContext _context = context;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Result> Handle(RegisterNotificationDeviceCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;

        if (!userId.HasValue)
            return Result.Failure(UserErrors.Unauthorized);

        var device = await _context.UserNotificationDevices
            .FirstOrDefaultAsync(x => x.FcmToken == request.FcmToken.Trim(), cancellationToken);

        if (device is null)
        {
            device = UserNotificationDevice.Create(userId.Value, request.FcmToken.Trim());

            _context.UserNotificationDevices.Add(device);
        }
        else if (device.UserId != userId.Value)
            device.ChangeUser(userId.Value);
        else
            device.MarkAsSeen();

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
