using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;
using Application.Reportes.DTOs;
using Application.Users.Interfaces;

namespace Application.Reportes.UseCases;

public class GetReporteElectoresPorOperador
{
    private readonly IOperadorPersonaRepository _operadorElectorRepository;
    private readonly IUserRepository            _userRepository;
    private readonly ICurrentUserService        _currentUserService;

    public GetReporteElectoresPorOperador(
        IOperadorPersonaRepository operadorElectorRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _userRepository            = userRepository;
        _currentUserService        = currentUserService;
    }

    public async Task<ReporteElectoresResult> ExecuteAsync(int? operadorIdParam, CancellationToken cancellationToken = default)
    {
        var requesterId   = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;
        var tenantId      = _currentUserService.TenantId;

        // Operador ignora el parámetro y siempre usa su propia sesión
        var operadorId = requesterRole == Operator ? requesterId : (operadorIdParam ?? requesterId);

        var operador = await _userRepository.GetByIdAsync(operadorId, cancellationToken);
        if (operador is null || operador.TenantId != tenantId)
            throw new KeyNotFoundException("Operador no encontrado.");

        var autorizado = requesterRole switch
        {
            Admin       => true,
            Coordinator => operador.CoordinatorId == requesterId,
            Operator    => true, // ya forzamos su propio id arriba
            _           => false
        };

        if (!autorizado)
            throw new UnauthorizedAccessException("No tiene permiso para generar este reporte.");

        var electores = await _operadorElectorRepository.GetByOperadorAsync(operadorId, cancellationToken);

        var dtos = electores.Select(e => new ElectorReporteDto(
            e.NumeroCed,
            e.Nombre   ?? "",
            e.Apellido ?? "",
            e.LocalVotacion,
            e.Mesa,
            e.Orden
        ));

        return new ReporteElectoresResult(operador.FullName, dtos);
    }
}
