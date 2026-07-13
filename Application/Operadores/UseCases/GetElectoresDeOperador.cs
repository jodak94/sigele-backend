using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.DTOs;
using Application.Operadores.Interfaces;
using Application.Users.Interfaces;

namespace Application.Operadores.UseCases;

public class GetElectoresDeOperador
{
    private readonly IOperadorPersonaRepository _operadorElectorRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly ITenantService _tenantService;

    public GetElectoresDeOperador(
        IOperadorPersonaRepository operadorElectorRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        ITenantService tenantService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _tenantService = tenantService;
    }

    public async Task<IEnumerable<OperadorElectorDto>> ExecuteAsync(int operadorId, CancellationToken cancellationToken = default)
    {
        var requesterId = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;
        var tenantId = _currentUserService.TenantId;

        var operador = await _userRepository.GetByIdAsync(operadorId, cancellationToken);
        if (operador is null || operador.TenantId != tenantId)
            throw new KeyNotFoundException("Operador no encontrado.");

        var autorizado = requesterRole switch
        {
            Admin       => true,
            Coordinator => operador.CoordinatorId == requesterId,
            Operator    => operadorId == requesterId,
            _           => false
        };

        if (!autorizado)
            throw new UnauthorizedAccessException("No tiene permiso para ver los electores de este operador.");

        var subdomain = _tenantService.GetCurrentTenantSubdomain();
        var electores = await _operadorElectorRepository.GetByOperadorAsync(operadorId, cancellationToken);

        return electores.Select(e => e with
        {
            MensajeWhatsapp = BuildMensajeWhatsapp(e, subdomain),
            TelefonoWhatsapp = NormalizarTelefonoWhatsapp(e.NroTelefono)
        });
    }

    private const string CodigoPaisPy = "595";

    private static string? NormalizarTelefonoWhatsapp(string? telefono)
    {
        if (string.IsNullOrWhiteSpace(telefono))
            return null;

        var soloDigitos = new string(telefono.Where(char.IsDigit).ToArray());
        if (soloDigitos.Length == 0)
            return null;

        if (soloDigitos.StartsWith(CodigoPaisPy))
            return soloDigitos;

        if (soloDigitos.StartsWith("0"))
            soloDigitos = soloDigitos.TrimStart('0');

        return CodigoPaisPy + soloDigitos;
    }

    private static string BuildMensajeWhatsapp(OperadorElectorDto e, string subdomain)
    {
        var nombreElector = $"{e.Nombre} {e.Apellido}".Trim();
        var local = string.IsNullOrWhiteSpace(e.LocalVotacion) ? "-" : e.LocalVotacion;
        var ciudad = string.IsNullOrWhiteSpace(e.Ciudad) ? "-" : e.Ciudad;
        var depto = string.IsNullOrWhiteSpace(e.Departamento) ? "-" : e.Departamento;
        var mesa = e.Mesa?.ToString() ?? "-";
        var orden = e.Orden?.ToString() ?? "-";

        return
            $"{nombreElector}\n" +
            $"*Local de Votacion*\n" +
            $"{local}\n" +
            $"{ciudad}- {depto}\n" +
            $"*Mesa:* {mesa}\n" +
            $"*Orden:* {orden}\n" +
            $"https://{subdomain}.sigele.com.py/sobre-mi";
    }
}
