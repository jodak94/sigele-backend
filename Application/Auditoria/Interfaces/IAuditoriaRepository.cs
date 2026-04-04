using Application.Auditoria.DTOs;

namespace Application.Auditoria.Interfaces;

public interface IAuditoriaRepository
{
    Task<IEnumerable<AlertaOperadorDto>> GetCaptacionesRapidasAsync(int tenantId, int? coordinadorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AlertaOperadorDto>> GetMismaCoordenadaAsync(int tenantId, int? coordinadorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AlertaOperadorDto>> GetFueraHorarioAsync(int tenantId, int? coordinadorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<AlertaOperadorDto>> GetUbicacionDenegadaAsync(int tenantId, int? coordinadorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<MapaOperadorDto>> GetMapaOperadorAsync(int tenantId, int operadorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CaptacionDetalleDto>> GetCaptacionesDeOperadorAsync(int tenantId, int operadorId, string tipoAlerta, CancellationToken cancellationToken = default);
}
