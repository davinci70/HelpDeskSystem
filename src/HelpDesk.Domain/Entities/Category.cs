namespace HelpDesk.Domain.Entities;

public sealed class Category : Entity
{
    public string Name { get; private set; } = null!;

    public Category()
    {
        
    }

    public Category(string name)
    {
        SetName(name);
    }

    public void UpdateName(string name)
    {
        SetName(name);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name cannot be null or empty.", nameof(name));
        }

        Name = name.Trim();
    }
}
