using Domain.Entities;

namespace Application.Roles;

public interface IRoleRepository
{
    Task<IEnumerable<Role>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Role?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<string>> GetPermissionsByRoleIdAsync(int roleId, CancellationToken cancellationToken = default);
}