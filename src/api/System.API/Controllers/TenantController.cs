using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Domain.DTOs.Tenants;
using System.Domain.Models;
using System.Infrastructure.Database;

namespace System.API.Controllers;
[Route("tenant-v2")]
[ApiController]
internal sealed class TenantController(SystemDbContext systemDbContext) : ControllerBase
{
    [HttpGet("getall")]
    public async Task<ActionResult<CollectionDto>> GetTenants()
    {
        List<TenantDto> tenants = await systemDbContext
            .Tenants
            .Select(TenantQueries.ProjectToDto())
            .ToListAsync();

        CollectionDto response = new()
        {
            Data = tenants
        };

        return Ok(response);
    }

    [HttpGet("SSS/{id}")]
    public async Task<ActionResult<TenantDto>> GetTenant(int id)
    {
        TenantDto? tenant = await systemDbContext
            .Tenants
            .Where(_ => _.TenantId == id)
            .Select(TenantQueries.ProjectToDto())
            .FirstOrDefaultAsync();

        if (tenant == null)
        {
            return NotFound();
        }

        return Ok(tenant);
    }

    [HttpPost("sS")]
    public async Task<ActionResult<TenantDto>> CreateTenant(CreateTenantDto createTenantDto)
    {
        TenantM tenant = createTenantDto.ToEntity();

        systemDbContext.Tenants.Add(tenant);

        await systemDbContext.SaveChangesAsync();

        TenantDto tenantDto = tenant.ToDto();

        return CreatedAtAction(nameof(CreateTenant), new { tenanName = tenantDto.TenantName }, tenantDto);
    }

    [HttpPut("SWWS/{id}")]
    public async Task<ActionResult> UpdateTenant(int id, CreateTenantDto dto)
    {
        TenantM? tenant = await systemDbContext.Tenants.FirstOrDefaultAsync(h => h.TenantId == id);

        if (tenant is null)
        {
            return NotFound();
        }

        tenant.TenantName = dto.TenantName!;
        tenant.DatabaseName = dto.DatabaseName!;

        await systemDbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("SSWS/{id}")]
    public async Task<ActionResult> DeleteHabit(int id)
    {
        TenantM? tenant = await systemDbContext.Tenants.FirstOrDefaultAsync(h => h.TenantId == id);

        if (tenant is null)
        {
            return NotFound();
        }

        systemDbContext.Tenants.Remove(tenant);

        await systemDbContext.SaveChangesAsync();

        return NoContent();
    }
}