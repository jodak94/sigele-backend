using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Reportes.DTOs;
using Application.Vehiculos.Interfaces;

namespace Application.Reportes.UseCases;

public class GetReporteVehiculos
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetReporteVehiculos(IVehiculoRepository vehiculoRepository, ICurrentUserService currentUserService)
    {
        _vehiculoRepository = vehiculoRepository;
        _currentUserService = currentUserService;
    }

    public async Task<VehiculosReporteData> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        if (_currentUserService.Role == Operator)
            throw new UnauthorizedAccessException("No tiene permiso para generar este reporte.");

        var vehiculos = await _vehiculoRepository.GetAllForReporteAsync(cancellationToken);

        var items = vehiculos.Select(v => new VehiculoReporteItemDto(
            v.NombreDueno,
            v.TelefonoDueno,
            v.Capacidad,
            v.MontoAlquiler,
            v.Operador?.FullName,
            v.Observacion
        )).ToList();

        return new VehiculosReporteData(items, items.Count);
    }
}
