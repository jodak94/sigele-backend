using Application.Common.Interfaces;
using Application.Operadores.Interfaces;

namespace Application.Operadores.UseCases;

public class RemoverElector
{
    private readonly IOperadorElectorRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public RemoverElector(IOperadorElectorRepository repository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task ExecuteAsync(int electorId, CancellationToken cancellationToken = default)
    {
        var operadorId = _currentUserService.UserId;

        var registro = await _repository.GetAsync(operadorId, electorId, cancellationToken);
        if (registro is null || !registro.IsActive)
            throw new KeyNotFoundException("Asignación no encontrada.");

        registro.IsActive   = false;
        registro.DeletedAt  = DateTimeOffset.UtcNow;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
