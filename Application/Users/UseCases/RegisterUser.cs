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

    public RegisterUser(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IRoleRepository roleRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserResponseDto> ExecuteAsync(CreateUserDto dto,
        CancellationToken cancellationToken = default)
    {
        if (await _userRepository.ExistByEmailAsync(dto.Email, cancellationToken)) {
            throw new InvalidOperationException("A user with this email already exists.");
        }
        
        var role = await _roleRepository.GetByIdAsync(dto.RoleId, cancellationToken);
        if (role is null)
        {
            throw new InvalidOperationException("The role does not exist.");
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            Phone = dto.Phone,
            Password = _passwordHasher.Hash(dto.Password),
            RoleId = role.Id,
            CreatedBy = 0
        };

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Phone = user.Phone,
            Role = new RoleDto()
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = role.Permissions.Select(p => p.Name)
            }
        };
    }
}