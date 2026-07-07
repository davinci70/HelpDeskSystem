using HelpDesk.Application.Errors;

namespace HelpDesk.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteCategoryCommand, Result>
{
    private readonly IApplicationDbContext _context = context;

    public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            return Result.Failure(CategoryErrors.CategoryNotFound);

        var inUse = await _context.Tickets.AnyAsync(t => t.CategoryId == request.Id, cancellationToken);

        if (inUse)
            return Result.Failure(CategoryErrors.InUse);

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
