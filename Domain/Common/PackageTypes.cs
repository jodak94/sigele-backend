namespace Domain.Common;

public static class PackageTypes
{
    public const string Base       = "BASE";
    public const string Bloque1000 = "BLOQUE_1000";
    public const string Bloque5000 = "BLOQUE_5000";
    public const string Full       = "FULL";

    public static readonly IReadOnlyDictionary<string, string> Labels = new Dictionary<string, string>
    {
        [Base]       = "Plan fundacional",
        [Bloque1000] = "+1.000 electores",
        [Bloque5000] = "+5.000 electores",
        [Full]       = "Plan full — sin límite",
    };

    public static bool IsValid(string packageType) => Labels.ContainsKey(packageType);
}
