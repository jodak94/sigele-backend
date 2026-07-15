using Application.Asistencia.DTOs;
using Application.Common.DTOs;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;

namespace Application.Asistencia.UseCases;

public class GetAsistenciaList
{
    private readonly IOperadorPersonaRepository _operadorPersonaRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAsistenciaList(IOperadorPersonaRepository operadorPersonaRepository, ICurrentUserService currentUserService)
    {
        _operadorPersonaRepository = operadorPersonaRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedResultDto<AsistenciaElectorDto>> ExecuteAsync(PaginationQueryDto query, string? search, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _operadorPersonaRepository.GetAsistenciaListAsync(
            _currentUserService.TenantId, search, query.Page, query.PageSize, cancellationToken);

        return new PaginatedResultDto<AsistenciaElectorDto>
        {
            Items = items,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}
