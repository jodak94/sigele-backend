namespace Application.Auditoria.Constants;

public static class TiposAlerta
{
    public const string CaptacionesRapidas  = "captaciones_rapidas";
    public const string MismaCoordenada     = "misma_coordenada";
    public const string FueraHorario        = "fuera_horario";
    public const string UbicacionDenegada   = "ubicacion_denegada";

    public static readonly IReadOnlyDictionary<string, string> Descripciones =
        new Dictionary<string, string>
        {
            [CaptacionesRapidas] = "El operador registró 30 o más captaciones en una ventana de 15 minutos.",
            [MismaCoordenada]    = "El operador registró 15 o más captaciones desde la misma coordenada.",
            [FueraHorario]       = "El operador registró captaciones entre las 00:00 y las 05:00 (hora Paraguay).",
            [UbicacionDenegada]  = "El operador registró captaciones sin compartir su ubicación."
        };
}
