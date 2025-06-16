namespace Tenant.Domain.Entities.INCategory;

public sealed class INCategoryM : AggregateRoot
{
    public int CategoryId { get; set; }
    public required string CategoryCode { get; set; }
    public required string CategoryDesc { get; set; }
    public static INCategoryM Create
    (
        string categoryCode,
        string categoryDesc
    )
    {
        INCategoryM obj = new()
        {
            CategoryCode = categoryCode,
            CategoryDesc = categoryDesc,
        };

        return obj;
    }
}