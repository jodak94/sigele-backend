using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;
using Application.Reportes.DTOs;

namespace Application.Reportes.UseCases;

public class GetReporteCandidatosMesa
{
    private readonly IOperadorPersonaRepository _operadorElectorRepository;
    private readonly ICurrentUserService        _currentUserService;

    public GetReporteCandidatosMesa(
        IOperadorPersonaRepository operadorElectorRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _currentUserService        = currentUserService;
    }

    public async Task<CandidatosMesaResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var requesterId   = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;

        if (requesterRole == Operator)
            throw new UnauthorizedAccessException("No tiene permiso para generar este reporte.");

        int? coordinatorId = requesterRole == Coordinator ? requesterId : null;

        var flat = await _operadorElectorRepository.GetCandidatosMesaFlatAsync(coordinatorId, _currentUserService.TenantId, cancellationToken);

        var locales = flat
            .GroupBy(x => x.LocalVotacion?.Trim() is { Length: > 0 } nombre ? nombre : "Sin Local Asignado")
            .OrderBy(g => g.Key)
            .Select(g => new CandidatoMesaLocalDto(
                g.Key,
                g.OrderBy(x => x.Mesa).ThenBy(x => x.NombreApellido)
                  .Select(x => new CandidatoMesaItemDto(
                      x.NombreApellido, x.NroCedula, x.Telefono, x.Mesa, x.OperadorResponsable
                  ))
            ));

        return new CandidatosMesaResult(locales);
    }
}
