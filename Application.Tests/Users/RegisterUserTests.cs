using Application.Common.Interfaces;
using Application.Roles;
using Application.Users.DTOs;
using Application.Users.Interfaces;
using Application.Users.UseCases;
using Moq;

namespace Application.Tests.Users;

public class RegisterUserTests
{
    private readonly Mock<IUserRepository> _userRepo = new();
    private readonly Mock<IRoleRepository> _roleRepo = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ITenantService> _tenantService = new();
    private readonly Mock<ICurrentUserService> _currentUserService = new();

    private RegisterUser CreateUseCase() => new(
        _userRepo.Object,
        _unitOfWork.Object,
        _passwordHasher.Object,
        _roleRepo.Object,
        _tenantService.Object,
        _currentUserService.Object
    );

    [Fact]
    public async Task ExecuteAsync_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        var dto = new CreateUserDto { Email = "existing@test.com", RoleId = 3 };

        _userRepo
            .Setup(r => r.ExistByEmailAsync(dto.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var useCase = CreateUseCase();

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(
            () => useCase.ExecuteAsync(dto)
        );

        Assert.Equal("A user with this email already exists.", ex.Message);
    }
}
