using Common.Domain.Abstractions;

namespace Parent.Domain.Inventory.Category;

public sealed class CategoryM : AggregateRoot
{
    public int CategoryId { get; set; }
    public required string CategoryCode { get; set; }
    public required string CategoryDesc { get; set; }
    public static CategoryM Create
    (
        string categoryCode,
        string categoryDesc
    )
    {
        CategoryM obj = new()
        {
            CategoryCode = categoryCode,
            CategoryDesc = categoryDesc,
        };

        return obj;
    }
}