namespace Domain.Entities;

public class SeccLocal
{
    public short CodigoDep { get; set; }
    public short CodigoDis { get; set; }
    public short CodigoSec { get; set; }
    public short CodigoLoc { get; set; }
    public short? CodLocal { get; set; }
    public int SeccLoc { get; set; }

    public Seccional Seccional { get; set; } = null!;
    public LocalVotacion Local { get; set; } = null!;
}
