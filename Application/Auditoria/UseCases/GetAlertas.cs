using static Application.Common.Constants.Roles;
using Application.Auditoria.Constants;
using Application.Auditoria.DTOs;
using Application.Auditoria.Interfaces;
using Application.Common.Interfaces;

namespace Application.Auditoria.UseCases;

public class GetAlertas
{
    private readonly IAuditoriaRepository _repository;
    private readonly ICurrentUserService  _currentUserService;

    public GetAlertas(IAuditoriaRepository repository, ICurrentUserService currentUserService)
    {
        _repository         = repository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<AlertaDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = _currentUserService.TenantId;
        var role     = _currentUserService.Role;

        int? coordinadorId = role switch
        {
            Admin       => null,
            Coordinator => _currentUserService.UserId,
            _           => throw new UnauthorizedAccessException("No tiene permiso para ver las alertas.")
        };

        var captacionesRapidas = await _repository.GetCaptacionesRapidasAsync(tenantId, coordinadorId, cancellationToken);
        var mismaCoordenada    = await _repository.GetMismaCoordenadaAsync(tenantId, coordinadorId, cancellationToken);
        var fueraHorario       = await _repository.GetFueraHorarioAsync(tenantId, coordinadorId, cancellationToken);
        var ubicacionDenegada  = await _repository.GetUbicacionDenegadaAsync(tenantId, coordinadorId, cancellationToken);

        return BuildAlertas(TiposAlerta.CaptacionesRapidas, captacionesRapidas)
            .Concat(BuildAlertas(TiposAlerta.MismaCoordenada,   mismaCoordenada))
            .Concat(BuildAlertas(TiposAlerta.FueraHorario,      fueraHorario))
            .Concat(BuildAlertas(TiposAlerta.UbicacionDenegada, ubicacionDenegada));
    }

    private static IEnumerable<AlertaDto> BuildAlertas(string tipo, IEnumerable<AlertaOperadorDto> operadores) =>
        operadores.Select(o => new AlertaDto(tipo, TiposAlerta.Descripciones[tipo], o));
}
