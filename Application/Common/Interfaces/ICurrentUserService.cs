using Domain.Entities;

namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    int UserId { get; }
    int TenantId { get; }
    string Role { get; }
    IEnumerable<string> Permissions { get; }
    bool HasPermission(string permission);
}