namespace HelpDesk.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(
    int Id
) : ICommand;
