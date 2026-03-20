namespace Domain.Entities;

public class LocalVotacion
{
    public int SeccLoc { get; set; }
    public string? NombreLoc { get; set; }
    public string? Direccion { get; set; }
    public string? Recibido { get; set; }

    public ICollection<SeccLocal> SeccLocales { get; set; } = new List<SeccLocal>();
    public ICollection<Elector> Electores { get; set; } = new List<Elector>();
}
