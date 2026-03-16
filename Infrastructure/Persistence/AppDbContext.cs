using System.Linq.Expressions;
using System.Reflection;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    private readonly ITenantService _tenantService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ITenantService tenantService) : base(options)
    {
        _tenantService = tenantService;
    }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    
    private int CurrentTenantId {
        get {
            try { return _tenantService.GetCurrentTenantId(); }
            catch { return 0; }
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder){

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        ApplyGlobalFilters(modelBuilder);
    }

    private void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        //Reflection to add tenantId and isActive filter globally
        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
            if(!typeof(AuditableEntity).IsAssignableFrom(entityType.ClrType))
                continue;

            var parameter = Expression.Parameter(entityType.ClrType, "e");
            
            var isActiveProperty = Expression.Property(parameter, nameof(AuditableEntity.IsActive));
            var isActiveFilter = Expression.Equal(isActiveProperty, Expression.Constant(true));
            
            var tenantIdProperty = Expression.Property(parameter, nameof(AuditableEntity.TenantId));
            var tenantIdFilter = Expression.Equal(
                tenantIdProperty,
                Expression.Property(
                    Expression.Constant(this), 
                    typeof(AppDbContext).GetProperty(
                        nameof(CurrentTenantId), BindingFlags.NonPublic | BindingFlags.Instance)!
                )
            );
            
            var combinedFilter = Expression.AndAlso(isActiveFilter, tenantIdFilter);
            var lambda = Expression.Lambda(combinedFilter, parameter);
            modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
        }
    }
}