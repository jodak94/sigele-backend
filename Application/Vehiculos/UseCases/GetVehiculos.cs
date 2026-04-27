using Application.Common.DTOs;
using Application.Vehiculos.DTOs;
using Application.Vehiculos.Interfaces;

namespace Application.Vehiculos.UseCases;

public class GetVehiculos
{
    private readonly IVehiculoRepository _vehiculoRepository;

    public GetVehiculos(IVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    public async Task<PaginatedResultDto<VehiculoDto>> ExecuteAsync(PaginationQueryDto query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _vehiculoRepository.GetAllAsync(query.Page, query.PageSize, cancellationToken);

        var dtos = items.Select(v => new VehiculoDto(
            v.Id,
            v.Capacidad,
            v.NombreDueno,
            v.TelefonoDueno,
            v.OperadorId,
            v.Operador?.FullName,
            v.MontoAlquiler,
            v.Observacion
        ));

        return new PaginatedResultDto<VehiculoDto>
        {
            Items = dtos,
            Page = query.Page,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }
}
