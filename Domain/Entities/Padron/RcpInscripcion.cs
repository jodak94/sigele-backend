namespace Domain.Entities.Padron;

public class RcpInscripcion
{
    public long Id { get; set; }
    public int? Cedula { get; set; }
    public short? Depart { get; set; }
    public short? Distrito { get; set; }
    public short? Zona { get; set; }
    public short? Local { get; set; }
    public DateOnly? FecInscri { get; set; }
    public string? Tipo { get; set; }
    public string? Menores { get; set; }
    public string? Interdicto { get; set; }
    public string? PolMil { get; set; }

    public Persona? Persona { get; set; }
    public RcpLocalidad? Localidad { get; set; }
    public RcpZona? ZonaNav { get; set; }
}
