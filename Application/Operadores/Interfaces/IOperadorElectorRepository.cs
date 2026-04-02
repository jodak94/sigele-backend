using Application.Operadores.DTOs;
using Application.Reportes.DTOs;
using Domain.Entities;

namespace Application.Operadores.Interfaces;

public interface IOperadorElectorRepository
{
    Task<bool> ElectorActivoEnTenantAsync(int electorId, int tenantId, CancellationToken cancellationToken = default);
    Task AddAsync(OperadorElector operadorElector, CancellationToken cancellationToken = default);
    Task<IEnumerable<OperadorElectorDto>> GetByOperadorAsync(int operadorId, CancellationToken cancellationToken = default);
    Task<OperadorElector?> GetAsync(int operadorId, int electorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<OperadorInfoDto>> GetInfoDeOperadoresAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ElectorAsignadoDto>> BuscarPorNumeroCedAsync(int numeroCed, int? operatorId, int? coordinatorId, int tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ResumenOperadorItemDto>> GetResumenOperadoresAsync(int? coordinatorId, int tenantId, short? codigoSeccional, CancellationToken cancellationToken = default);
    Task<IEnumerable<DiaDFlatItemDto>> GetDiaDFlatAsync(int? coordinatorId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CandidatoMesaFlatItemDto>> GetCandidatosMesaFlatAsync(int? coordinatorId, CancellationToken cancellationToken = default);
    Task<OperadorElector?> GetByUserAndElectorAsync(int userId, int electorId, bool includeInactive, CancellationToken cancellationToken = default);
    Task<IEnumerable<ResumenCoordinadorDto>> GetResumenCoordinadoresAsync(int tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ElectorUbicacionDto>> GetElectorUbicacionesAsync(int? operadorId, int? coordinadorId, int tenantId, CancellationToken cancellationToken = default);
}
