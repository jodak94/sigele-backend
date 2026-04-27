using Application.Vehiculos.DTOs;
using Application.Vehiculos.Interfaces;

namespace Application.Vehiculos.UseCases;

public class GetVehiculoById
{
    private readonly IVehiculoRepository _vehiculoRepository;

    public GetVehiculoById(IVehiculoRepository vehiculoRepository)
    {
        _vehiculoRepository = vehiculoRepository;
    }

    public async Task<VehiculoDto> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var vehiculo = await _vehiculoRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"Vehículo con id {id} no encontrado.");

        return new VehiculoDto(
            vehiculo.Id,
            vehiculo.Capacidad,
            vehiculo.NombreDueno,
            vehiculo.TelefonoDueno,
            vehiculo.OperadorId,
            vehiculo.Operador?.FullName,
            vehiculo.MontoAlquiler,
            vehiculo.Observacion
        );
    }
}
