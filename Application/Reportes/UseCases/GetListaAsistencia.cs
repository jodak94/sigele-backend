using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;
using Application.Reportes.DTOs;
using Application.Users.Interfaces;

namespace Application.Reportes.UseCases;

public class GetListaAsistencia
{
    private readonly IOperadorElectorRepository _operadorElectorRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetListaAsistencia(
        IOperadorElectorRepository operadorElectorRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ListaAsistenciaResult> ExecuteAsync(int operadorId, CancellationToken cancellationToken = default)
    {
        var requesterId   = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;
        var tenantId      = _currentUserService.TenantId;

        var operador = await _userRepository.GetByIdAsync(operadorId, cancellationToken);
        if (operador is null || operador.TenantId != tenantId)
            throw new KeyNotFoundException("Operador no encontrado.");

        var autorizado = requesterRole switch
        {
            Admin       => true,
            Coordinator => operador.CoordinatorId == requesterId,
            Operator    => operadorId == requesterId,
            _           => false
        };

        if (!autorizado)
            throw new UnauthorizedAccessException("No tiene permiso para generar este reporte.");

        var items = await _operadorElectorRepository.GetListaAsistenciaAsync(operadorId, cancellationToken);

        return new ListaAsistenciaResult(operador.FullName, items);
    }
}
