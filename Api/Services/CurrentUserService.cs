using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Application.Common.Interfaces;

namespace Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId =>
        int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);
    
    public string Role => _httpContextAccessor.HttpContext!.User.FindFirst("role")!.Value;

    public int TenantId => int.Parse(_httpContextAccessor.HttpContext!.User.FindFirst("tenantId")!.Value);

    public IEnumerable<string> Permissions
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext!.User.FindFirst("permissions")?.Value;

            return claim is null ? Enumerable.Empty<string>() : JsonSerializer.Deserialize<IEnumerable<string>>(claim)!;
        }
    }

    public bool HasPermission(string permission) => Permissions.Contains(permission);
}