using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public class ActualizarElector
{
    private readonly IOperadorElectorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public ActualizarElector(IOperadorElectorRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int electorId, ActualizarElectorDto dto, CancellationToken cancellationToken = default)
    {
        var operadorId = _currentUserService.UserId;

        var registro = await _repository.GetAsync(operadorId, electorId, cancellationToken);
        if (registro is null || !registro.IsActive)
            throw new KeyNotFoundException("Asignación no encontrada.");

        registro.DisponibleMiembroMesa = dto.DisponibleMiembroMesa;
        registro.RequiereTransporte    = dto.RequiereTransporte;
        registro.NroTelefono           = dto.NroTelefono;
        registro.DireccionRecogida     = dto.DireccionRecogida;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
