using static Application.Common.Constants.Roles;
using Application.Auditoria.DTOs;
using Application.Auditoria.Interfaces;
using Application.Common.Interfaces;
using Application.Users.Interfaces;

namespace Application.Auditoria.UseCases;

public class GetCaptacionesDeOperador
{
    private readonly IAuditoriaRepository _repository;
    private readonly IUserRepository      _userRepository;
    private readonly ICurrentUserService  _currentUserService;

    public GetCaptacionesDeOperador(
        IAuditoriaRepository repository,
        IUserRepository      userRepository,
        ICurrentUserService  currentUserService)
    {
        _repository         = repository;
        _userRepository     = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<CaptacionDetalleDto>> ExecuteAsync(int operadorId, string tipoAlerta, CancellationToken cancellationToken = default)
    {
        var tenantId = _currentUserService.TenantId;
        var role     = _currentUserService.Role;

        if (role == Coordinator)
        {
            var operador = await _userRepository.GetByIdAsync(operadorId, cancellationToken);
            if (operador is null || operador.CoordinatorId != _currentUserService.UserId)
                throw new UnauthorizedAccessException("No tiene permiso para ver este operador.");
        }
        else if (role != Admin)
        {
            throw new UnauthorizedAccessException("No tiene permiso para ver esta información.");
        }

        return await _repository.GetCaptacionesDeOperadorAsync(tenantId, operadorId, tipoAlerta, cancellationToken);
    }
}
