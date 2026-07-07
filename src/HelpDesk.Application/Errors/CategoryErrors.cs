namespace HelpDesk.Application.Errors;


public static class CategoryErrors
{
    public static Error AlreadyExists =
        new( "Categories.AlreadyExists","category already exists.", StatusCodes.Status409Conflict);
    
    public static Error CategoryNotFound =
        new( "Categories.NotFound", "The specified category does not exist.", StatusCodes.Status404NotFound);

    public static readonly Error InUse =
        new( "Categories.InUse", "The category cannot be deleted because it is assigned to one or more tickets.",
            StatusCodes.Status409Conflict);
} 