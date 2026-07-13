using Application.Operadores.DTOs;
using Application.Reportes.DTOs;
using Domain.Entities;

namespace Application.Operadores.Interfaces;

public interface IOperadorPersonaRepository
{
    Task<bool> PersonaActivaEnTenantAsync(int cedula, int tenantId, CancellationToken cancellationToken = default);
    Task AddAsync(OperadorPersona op, CancellationToken cancellationToken = default);
    Task<IEnumerable<OperadorElectorDto>> GetByOperadorAsync(int operadorId, CancellationToken cancellationToken = default);
    Task<OperadorPersona?> GetAsync(int operadorId, int cedula, CancellationToken cancellationToken = default);
    Task<IEnumerable<OperadorInfoDto>> GetInfoDeOperadoresAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ElectorAsignadoDto>> BuscarPorNumeroCedAsync(int numeroCed, int? operatorId, int? coordinatorId, int tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ResumenOperadorItemDto>> GetResumenOperadoresAsync(int? coordinatorId, int tenantId, short? codigoSeccional, CancellationToken cancellationToken = default);
    Task<IEnumerable<DiaDFlatItemDto>> GetDiaDFlatAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CandidatoMesaFlatItemDto>> GetCandidatosMesaFlatAsync(int? coordinatorId, int tenantId, CancellationToken cancellationToken = default);
    Task<OperadorPersona?> GetByUserAndPersonaAsync(int userId, int cedula, bool includeInactive, CancellationToken cancellationToken = default);
    Task<IEnumerable<ResumenCoordinadorDto>> GetResumenCoordinadoresAsync(int tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ElectorUbicacionDto>> GetElectorUbicacionesAsync(int? operadorId, int? coordinadorId, int tenantId, CancellationToken cancellationToken = default);
}
