using HelpDesk.Application.Common.Interfaces;

namespace HelpDesk.Application.Authentication.Register;

public class RegisterCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher) : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var IsEmailExist = await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);

        if (IsEmailExist)
            return Result.Failure<RegisterResponse>(UserErrors.EmailAlreadyExists);

        var hashedPassword = _passwordHasher.Hash(request.Password);

        var user = request.Adapt<User>();
        user.PasswordHash = hashedPassword;

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success(user.Adapt<RegisterResponse>());
    }
}
