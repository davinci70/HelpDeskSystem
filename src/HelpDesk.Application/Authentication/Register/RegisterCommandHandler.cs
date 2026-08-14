using HelpDesk.Application.Common.Interfaces;

namespace HelpDesk.Application.Authentication.Register;

public sealed class RegisterCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher)
    : IRequestHandler<
        RegisterCommand,
        Result<RegisterResponse>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _context.Users.AnyAsync(user => user.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            return Result.Failure<RegisterResponse>(
                UserErrors.EmailAlreadyExists);
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = User.Create(
            request.FullName,
            request.Email,
            passwordHash);

        await _context.Users.AddAsync(user,cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Adapt<RegisterResponse>());
    }
}
