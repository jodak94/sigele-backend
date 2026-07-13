using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;
using Application.Reportes.DTOs;

namespace Application.Reportes.UseCases;

public class GetReporteDiaD
{
    private readonly IOperadorPersonaRepository _operadorElectorRepository;
    private readonly ICurrentUserService        _currentUserService;

    public GetReporteDiaD(
        IOperadorPersonaRepository operadorElectorRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _currentUserService        = currentUserService;
    }

    public async Task<DiaDResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var requesterId   = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;

        if (requesterRole == Operator)
            throw new UnauthorizedAccessException("No tiene permiso para generar este reporte.");

        int? coordinatorId = requesterRole == Coordinator ? requesterId : null;

        var flat = await _operadorElectorRepository.GetDiaDFlatAsync(coordinatorId, _currentUserService.TenantId, cancellationToken);

        var locales = flat
            .GroupBy(x => x.LocalVotacion?.Trim() is { Length: > 0 } nombre ? nombre : "Sin Local Asignado")
            .OrderBy(g => g.Key)
            .Select(g => new DiaDLocalDto(
                g.Key,
                g.OrderBy(x => x.Mesa).ThenBy(x => x.Orden)
                  .Select(x => new DiaDElectorItemDto(
                      x.Mesa, x.Orden, x.NroCedula, x.NombreApellido,
                      x.Telefono, x.DireccionRecogida, x.RequiereTransporte, x.OperadorResponsable
                  ))
            ));

        return new DiaDResult(locales);
    }
}
