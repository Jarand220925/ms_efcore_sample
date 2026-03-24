using System.Text.Json;
using NetTopologySuite.Features;
using NetTopologySuite.IO.Converters;

namespace ms_efcore_sample.models;

public class CoordinateGeojsonDto
{
    public string JsonWithSystemTextJson { get; set; }
    public CoordinateGeojsonDto(string name, string description, Coordinate coordinate)
    {
        var attributes = new AttributesTable();
        attributes.Add("Name", name);
        attributes.Add("Description", description);
        
        var feature = new Feature(coordinate.GeographyPoint, attributes);
        var featureCollection = new FeatureCollection { feature };
        
        var options = new JsonSerializerOptions
        {
            Converters = { new GeoJsonConverterFactory() },
            // Optional: for pretty formatting
            WriteIndented = true 
        };
        
        JsonWithSystemTextJson = JsonSerializer.Serialize(featureCollection, options);
        Console.WriteLine(JsonWithSystemTextJson);
    }
}