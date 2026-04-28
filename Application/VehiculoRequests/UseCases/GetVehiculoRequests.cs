using Application.Common.DTOs;
using Application.VehiculoRequests.DTOs;
using Application.VehiculoRequests.Interfaces;

namespace Application.VehiculoRequests.UseCases;

public class GetVehiculoRequests
{
    private readonly IVehiculoRequestRepository _repository;

    public GetVehiculoRequests(IVehiculoRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedResultDto<VehiculoRequestDto>> ExecuteAsync(PaginationQueryDto query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetAllAsync(query.Page, query.PageSize, cancellationToken);

        var dtos = items.Select(r => new VehiculoRequestDto(
            r.Id,
            r.ElectorId,
            r.OperadorId,
            r.Operador?.FullName ?? string.Empty,
            r.NombreDueno,
            r.TelefonoDueno,
            r.Capacidad,
            r.MontoAlquiler,
            r.Estado.ToString(),
            r.Observacion,
            r.AprobadoPor?.FullName,
            r.FechaResolucion,
            r.CreatetAt
        ));

        return new PaginatedResultDto<VehiculoRequestDto>
        {
            Items      = dtos,
            Page       = query.Page,
            PageSize   = query.PageSize,
            TotalCount = totalCount
        };
    }
}
