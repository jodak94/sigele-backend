using Application.Users.DTOs;
using Application.Users.Interfaces;

namespace Application.Users.UseCases;

public class GetCoordinators
{
    private readonly IUserRepository _userRepository;

    public GetCoordinators(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserListItemDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var coordinators = await _userRepository.GetCoordinatorsAsync(cancellationToken);

        return coordinators.Select(u => new UserListItemDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Phone = u.Phone,
            Role = u.Role.Name,
            LastLogin = u.LastLogin,
            CreatedAt = u.CreatetAt
        });
    }
}
