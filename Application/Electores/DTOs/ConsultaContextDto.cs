namespace Application.Electores.DTOs;

public record ConsultaContextDto(
    int TenantId,
    string IpCliente,
    string? UserAgent,
    string? Origin,
    string Host,
    string MetodoHttp);
