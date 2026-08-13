using HelpDesk.Application.Common.Interfaces;

namespace HelpDesk.Application.Authentication.Login;

public class LoginCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
    : IRequestHandler<LoginCommand, Result<string>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<Result<string>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user is null)
            return Result.Failure<string>(UserErrors.InvalidCredentials);

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash))
            return Result.Failure<string>(UserErrors.InvalidCredentials);

        var token = _jwtProvider.Generate(user);

        return Result.Success(token);
    }
}
