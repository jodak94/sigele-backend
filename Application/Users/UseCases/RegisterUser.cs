using static Application.Common.Constants.Roles;
using Application.Common.Constants;
using Application.Common.Interfaces;
using Application.Roles;
using Application.Users.DTOs;
using Application.Users.Interfaces;
using Domain.Entities;

namespace Application.Users.UseCases;

public class RegisterUser
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITenantService _tenantService;
    private readonly ICurrentUserService _currentUserService;

    public RegisterUser(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher,
        IRoleRepository roleRepository, ITenantService tenantService, ICurrentUserService currentUserService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tenantService = tenantService;
        _currentUserService = currentUserService;
    }

    public async Task<UserResponseDto> ExecuteAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.ExistByEmailAsync(dto.Email, cancellationToken))
            throw new InvalidOperationException("A user with this email already exists.");

        var role = await _roleRepository.GetByNameAsync(dto.RoleName, cancellationToken);
        if (role is null)
            throw new InvalidOperationException("The role does not exist.");

        if (role.Name == Admin)
            throw new UnauthorizedAccessException("Admin users cannot be created through this endpoint.");

        if (role.Name == Coordinator && !_currentUserService.HasPermission(Permissions.Users.CreateCoordinator))
            throw new UnauthorizedAccessException("You do not have permission to create coordinators.");

        var coordinatorId = ResolveCoordinatorId(role.Name, dto);

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            RoleId = role.Id,
            CoordinatorId = coordinatorId,
            TenantId = _tenantService.GetCurrentTenantId(),
            CreatedBy = _currentUserService.UserId
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Role = new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.Permissions.Select(p => p.Name)
            }
        };
    }

    private int? ResolveCoordinatorId(string roleName, CreateUserDto dto)
    {
        if (roleName != Operator)
            return null;

        if (_currentUserService.Role == Coordinator)
            return _currentUserService.UserId;

        // Admin must explicitly provide coordinatorId when creating an operator
        if (!dto.CoordinatorId.HasValue)
            throw new InvalidOperationException("A coordinator must be assigned when creating an operator.");

        return dto.CoordinatorId;
    }
}
