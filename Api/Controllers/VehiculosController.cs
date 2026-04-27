using Application.Common.Attributes;
using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Vehiculos.DTOs;
using Application.Vehiculos.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/vehiculos")]
[Authorize]
public class VehiculosController : ControllerBase
{
    private readonly CreateVehiculo _createVehiculo;
    private readonly UpdateVehiculo _updateVehiculo;
    private readonly DeleteVehiculo _deleteVehiculo;
    private readonly GetVehiculos _getVehiculos;
    private readonly GetVehiculoById _getVehiculoById;

    public VehiculosController(CreateVehiculo createVehiculo, UpdateVehiculo updateVehiculo,
        DeleteVehiculo deleteVehiculo, GetVehiculos getVehiculos, GetVehiculoById getVehiculoById)
    {
        _createVehiculo = createVehiculo;
        _updateVehiculo = updateVehiculo;
        _deleteVehiculo = deleteVehiculo;
        _getVehiculos = getVehiculos;
        _getVehiculoById = getVehiculoById;
    }

    [HttpGet]
    [RequiresPermission(Permissions.Vehiculos.Read)]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQueryDto query, CancellationToken cancellationToken)
    {
        var result = await _getVehiculos.ExecuteAsync(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [RequiresPermission(Permissions.Vehiculos.Read)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _getVehiculoById.ExecuteAsync(id, cancellationToken);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost]
    [RequiresPermission(Permissions.Vehiculos.Create)]
    public async Task<IActionResult> Create([FromBody] CreateVehiculoDto dto, CancellationToken cancellationToken)
    {
        var result = await _createVehiculo.ExecuteAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:int}")]
    [RequiresPermission(Permissions.Vehiculos.Update)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVehiculoDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _updateVehiculo.ExecuteAsync(id, dto, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    [RequiresPermission(Permissions.Vehiculos.Delete)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _deleteVehiculo.ExecuteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
