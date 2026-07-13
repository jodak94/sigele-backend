namespace Domain.Entities.Padron;

public class Persona
{
    public int Cedula { get; set; }
    public string? Nombre { get; set; }
    public string? Apellido { get; set; }
    public string? Sexo { get; set; }
    public DateOnly? FecNac { get; set; }
    public string Origen { get; set; } = string.Empty;

    public ICollection<RcpInscripcion> Inscripciones { get; set; } = new List<RcpInscripcion>();
}
