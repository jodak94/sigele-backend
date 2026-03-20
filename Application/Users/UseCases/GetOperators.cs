using static Application.Common.Constants.Roles;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Users.DTOs;
using Application.Users.Interfaces;

namespace Application.Users.UseCases;

public class GetOperators
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetOperators(IUserRepository userRepository, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedResultDto<UserListItemDto>> ExecuteAsync(PaginationQueryDto query,
        CancellationToken cancellationToken = default)
    {
        // coordinator sees only their operators, admin sees all
        var coordinatorId = _currentUserService.Role == Coordinator
            ? _currentUserService.UserId
            : (int?)null;

        var (items, totalCount) =
            await _userRepository.GetOperatorsAsync(coordinatorId, query.Page, query.PageSize, cancellationToken);

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
