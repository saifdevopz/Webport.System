using Common.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Domain.DataTransferObjects.Tenant;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public required string CategoryCode { get; set; }
    public required string CategoryDesc { get; set; }
}

public record CategoryWrapper<T>(T Category);
public record CategoriesWrapper<T>(IEnumerable<T> Category);