using Application.Common.Interfaces;
using Application.Operadores.Interfaces;
using Application.Reportes.DTOs;
using Application.Users.Interfaces;

namespace Application.Reportes.UseCases;

public class GetReporteElectoresPorOperador
{
    private readonly IOperadorElectorRepository _operadorElectorRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetReporteElectoresPorOperador(
        IOperadorElectorRepository operadorElectorRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
    }

    public async Task<ReporteElectoresResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var operadorId = _currentUserService.UserId;

        var operador = await _userRepository.GetByIdAsync(operadorId, cancellationToken);
        var electores = await _operadorElectorRepository.GetByOperadorAsync(operadorId, cancellationToken);

        var dtos = electores.Select(e => new ElectorReporteDto(
            e.Nombre ?? "",
            e.Apellido ?? "",
            e.NumeroCed,
            e.NroTelefono,
            e.DisponibleMiembroMesa,
            e.RequiereTransporte,
            e.DireccionRecogida
        ));

        return new ReporteElectoresResult(operador?.FullName ?? "Operador", dtos);
    }
}
