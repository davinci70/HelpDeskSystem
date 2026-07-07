using HelpDesk.Application.Abstractions.Persistence;
using HelpDesk.Application.Common.Results;
using HelpDesk.Application.Errors;

namespace HelpDesk.Application.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler(IApplicationDbContext context)
        : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryResponse>>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var categoryAlreadyExists = await _context.Categories.AnyAsync(c => c.Name == request.Name.Trim(), cancellationToken);

        if (categoryAlreadyExists)
            return Result.Failure<CreateCategoryResponse>(CategoryErrors.AlreadyExists);

        var category = new Category(request.Name.Trim());

        _context.Categories.Add(category);

        await _context.SaveChangesAsync(cancellationToken);

        var response = new CreateCategoryResponse(category.Id, category.Name);

        return Result.Success(response);
    }
}
