using Microsoft.EntityFrameworkCore;
using ShkoloASPNETcore.Infrastructure.Data;

namespace ShkoloASPNETcore.Tests.Infrastructure;

internal static class DbFactory
{
    public static ShkoloDbContext Create(string dbName)
    {
        var options = new DbContextOptionsBuilder<ShkoloDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ShkoloDbContext(options);
    }
}