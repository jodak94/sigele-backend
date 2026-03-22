namespace Application.Operadores.DTOs;

public record OperadorInfoDto(
    int UserId,
    string FullName,
    string Email,
    string Phone,
    int TotalElectores,
    int MiembrosMesa,
    int RequierenTransporte
);
