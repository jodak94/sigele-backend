namespace Domain.Entities;

public class Seccional
{
    public short CodigoDep { get; set; }
    public string? NDepart { get; set; }
    public short CodigoDis { get; set; }
    public string? NDistrito { get; set; }
    public short? Zona { get; set; }
    public short CodigoSec { get; set; }
    public string? Descripcio { get; set; }
    public string? WSeccio { get; set; }
    public string? Direccion { get; set; }

    public ICollection<SeccLocal> SeccLocales { get; set; } = new List<SeccLocal>();
}
