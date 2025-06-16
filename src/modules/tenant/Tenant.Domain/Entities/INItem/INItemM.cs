using Tenant.Domain.Entities.INCategory;

namespace Tenant.Domain.Entities.INItem;

public sealed class INItemM : AggregateRoot
{
    public int ItemId { get; set; }
    public int CategoryId { get; set; }
    public INCategoryM? Category { get; set; }
    public required string ItemCode { get; set; }
    public required string ItemDesc { get; set; }
    public static INItemM Create
    (
        string itemCode,
        string itemDesc
    )
    {
        INItemM obj = new()
        {
            ItemCode = itemCode,
            ItemDesc = itemDesc,
        };

        return obj;
    }
}