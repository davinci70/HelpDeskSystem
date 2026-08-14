using HelpDesk.Domain.Common.Primitives;

namespace HelpDesk.Domain.Entities;

public sealed class Category : AggregateRoot
{
    private Category()
    {
    }

    private Category(string name)
    {
        Name = NormalizeName(name);
    }

    public string Name { get; private set; } = string.Empty;

    public static Category Create(string name)
    {
        return new Category(name);
    }

    public void Rename(string name)
    {
        Name = NormalizeName(name);
    }

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Category name cannot be empty.",
                nameof(name));
        }

        return name.Trim();
    }
}
