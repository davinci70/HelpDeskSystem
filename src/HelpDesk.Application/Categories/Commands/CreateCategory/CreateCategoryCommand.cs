namespace HelpDesk.Application.Categories.Commands.CreateCategory;

public sealed record CreateCategoryCommand(string Name) : ICommand<CreateCategoryResponse>;
