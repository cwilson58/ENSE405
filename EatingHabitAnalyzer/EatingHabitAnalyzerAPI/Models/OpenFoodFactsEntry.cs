using System.Text.Json.Serialization;

namespace EatingHabitAnalyzerAPI.Models;

public class OpenFoodFactsEntry
{
    public string code { get; set; }

    public Product product { get; set; }
}

public class Product
{
    public string _id { get; set; }

    public dynamic _keywords { get; set; }

    public string product_name_en { get; set; }

    public string serving_size { get; set; }

    public Nutriments nutriments { get; set; }
}

public class Nutriments
{
    public double? alcohol_serving { get; set; }

    public double? carbohydrates_serving { get; set; }

    [JsonPropertyName("energy-kcal_serving")]
    public double? energy_kcal_serving { get; set; }

    public double? sodium_serving { get; set; }

    public double? salt_serving { get; set; }

    public double? sugars_serving { get; set; }

    public double? proteins_serving { get; set; }

    public double? fat_serving { get; set; }

    [JsonPropertyName("saturated-fat_serving")]
    public double? saturated_fat_serving { get; set; }

    public double? caffeine_serving { get; set; }

    public double? cholesterol_serving { get; set; }
}