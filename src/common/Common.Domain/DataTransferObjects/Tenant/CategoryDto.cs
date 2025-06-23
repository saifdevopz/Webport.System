namespace Common.Domain.DataTransferObjects.Tenant;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public required string CategoryCode { get; set; }
    public required string CategoryDesc { get; set; }
}

public record CategoryWrapper<T>(T Category);
public record CategoriesWrapper<T>(IEnumerable<T> Categories);