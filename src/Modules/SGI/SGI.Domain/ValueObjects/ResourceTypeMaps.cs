namespace SGI.Domain.ValueObjects;

public static class ResourceTypeMaps
{
    public static string ToSpanish(string code) => code switch
    {
        "ENERGY" => "Energía",
        "GAS" => "Gas",
        "WATER" => "Agua",
        _ => code
    };

    public static string ToCode(string value) => value switch
    {
        "Energía" => "ENERGY",
        "Gas" => "GAS",
        "Agua" => "WATER",
        _ => value
    };
}
