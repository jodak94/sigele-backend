namespace Application.Auditoria.DTOs;

public record AlertaOperadorDto(
    int    OperadorId,
    string OperadorNombre,
    string OperadorEmail
);

public record AlertaDto(
    string          TipoAlerta,
    string          Descripcion,
    AlertaOperadorDto Operador
);
