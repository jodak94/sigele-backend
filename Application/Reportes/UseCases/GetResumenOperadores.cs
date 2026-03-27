using static Application.Common.Constants.Roles;
using Application.Common.Interfaces;
using Application.Operadores.Interfaces;
using Application.Reportes.DTOs;
using Application.Users.Interfaces;

namespace Application.Reportes.UseCases;

public class GetResumenOperadores
{
    private readonly IOperadorElectorRepository _operadorElectorRepository;
    private readonly IUserRepository            _userRepository;
    private readonly ICurrentUserService        _currentUserService;

    public GetResumenOperadores(
        IOperadorElectorRepository operadorElectorRepository,
        IUserRepository userRepository,
        ICurrentUserService currentUserService)
    {
        _operadorElectorRepository = operadorElectorRepository;
        _userRepository            = userRepository;
        _currentUserService        = currentUserService;
    }

    public async Task<ResumenOperadoresData> ExecuteAsync(short? codigoSeccional, int? coordinadorIdFiltro, CancellationToken cancellationToken = default)
    {
        var requesterId   = _currentUserService.UserId;
        var requesterRole = _currentUserService.Role;
        var tenantId      = _currentUserService.TenantId;

        if (requesterRole == Operator)
            throw new UnauthorizedAccessException("No tiene permiso para generar este reporte.");

        int? coordinatorId = requesterRole switch
        {
            Coordinator => requesterId,
            Admin       => coordinadorIdFiltro,
            _           => null
        };

        var operadores = await _operadorElectorRepository.GetResumenOperadoresAsync(
            coordinatorId, tenantId, codigoSeccional, cancellationToken);

        var lista = operadores.ToList();

        var totales = new ResumenOperadoresTotalesDto(
            lista.Sum(x => x.ElectoresCaptados),
            lista.Sum(x => x.MiembrosMesaDisp),
            lista.Sum(x => x.ReqTransporte)
        );

        CoordinadorInfoDto? coordinador = null;
        int? resolvedCoordinatorId = requesterRole == Coordinator ? requesterId : coordinadorIdFiltro;
        if (resolvedCoordinatorId.HasValue)
        {
            var user = await _userRepository.GetByIdAsync(resolvedCoordinatorId.Value, cancellationToken);
            if (user is not null)
                coordinador = new CoordinadorInfoDto(user.Id, user.FullName, user.Phone);
        }

        return new ResumenOperadoresData(lista, totales, coordinador);
    }
}
