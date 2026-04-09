using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Users.DTOs;
using Application.Users.Interfaces;

namespace Application.Users.UseCases;

public class GetUsers
{
    private readonly IUserRepository _userRepository;

    public GetUsers(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<PaginatedResultDto<UserListItemDto>> ExecuteAsync(string requesterRole, PaginationQueryDto query, string? nombre, CancellationToken cancellationToken = default)
    {
        if (requesterRole != Common.Constants.Roles.Admin)
            throw new UnauthorizedAccessException("Solo el administrador puede listar todos los usuarios.");

        var (items, totalCount) = await _userRepository.GetAllAsync(query.Page, query.PageSize, nombre, cancellationToken);

        return new PaginatedResultDto<UserListItemDto>
        {
            Items = items.Select(u => new UserListItemDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role.Name,
                LastLogin = u.LastLogin,
                CreatedAt = u.CreatetAt
            }),
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}
