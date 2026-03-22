using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var basePath = Directory.GetCurrentDirectory();
        Console.WriteLine(basePath);
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                      .UseSnakeCaseNamingConvention();

        // mock tenant service for design time — migrations don't need a real tenant
        return new AppDbContext(optionsBuilder.Options, new DesignTimeTenantService());
    }
}

public class DesignTimeTenantService : ITenantService
{
    public int GetCurrentTenantId() => 0;
}