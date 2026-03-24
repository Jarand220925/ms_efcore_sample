using System.Text.Json;
using NetTopologySuite.Features;
using NetTopologySuite.IO.Converters;

namespace ms_efcore_sample.models;

public class CoordinateGeojsonDto
{
    public FeatureCollection FeatureCollection { get; set; }
    public CoordinateGeojsonDto(string name, string description, Coordinate coordinate)
    {
        var attributes = new AttributesTable();
        attributes.Add("Name", name);
        attributes.Add("Description", description);
        
        var feature = new Feature(coordinate.GeographyPoint, attributes);
        FeatureCollection = new FeatureCollection { feature };
        
    }
}