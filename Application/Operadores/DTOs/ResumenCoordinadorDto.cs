namespace Application.Operadores.DTOs;

public record ResumenCoordinadorDto(
    int    UserId,
    string FullName,
    string Email,
    string Phone,
    int    TotalOperadores,
    int    TotalElectores,
    int    TotalMiembrosMesa);
