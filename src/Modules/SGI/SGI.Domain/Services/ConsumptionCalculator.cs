namespace SGI.Domain.Services;

public static class ConsumptionCalculator
{
    /// <summary>
    /// Calcula el consumo diario según el tipo de recurso.
    /// </summary>
    /// <param name="resourceType">Tipo de recurso (Energía, Gas, Agua, etc.)</param>
    /// <param name="currentValue">Lectura actual (valor del medidor de hoy)</param>
    /// <param name="lastValue">Lectura anterior (valor del medidor de ayer)</param>
    /// <returns>Consumo diario calculado</returns>
    public static decimal CalculateDaily(string resourceType, decimal currentValue, decimal lastValue)
    {
        var diff = currentValue - lastValue;

        // Si la lectura es igual o menor → sin consumo
        if (diff <= 0)
            return 0;

        return resourceType switch
        {
            "Energía" => diff * 80m,   // ⚡ Energía → multiplicar por 80 → kWh
            "Gas" => diff * 1.210m,    // 🔥 Gas → multiplicar por 1.210 → m³
            "Agua" => diff,            // 💧 Agua → solo diferencia directa
            _ => diff                  // Otros recursos genéricos
        };
    }
}
