using Microsoft.EntityFrameworkCore;

namespace Common.Application.Database;

public interface IDbContextProvider
{
    DbContext GetContext();
}