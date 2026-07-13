namespace Domain.Entities.Padron;

public class RcpLocalidad
{
    public short Depart { get; set; }
    public short Distrito { get; set; }
    public short Zona { get; set; }
    public short Local { get; set; }
    public string? Descrip { get; set; }

    public RcpDepartamento? Dpto { get; set; }
    public RcpDistrito? DistNav { get; set; }
}
