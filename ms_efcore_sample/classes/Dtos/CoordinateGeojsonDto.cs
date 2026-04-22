using ms_efcore_sample.interfaces;
using ms_efcore_sample.models;
using NetTopologySuite.Features;

namespace ms_efcore_sample.classes.Dtos;

public class CoordinateGeojsonDto : IGeojsonDto
{
    public Feature Feature { get; set; }
    public FeatureCollection FeatureCollection { get; set; }
    public CoordinateGeojsonDto(Coordinate coordinate)
    {
        var id = coordinate.CoordinateId;
        var longitude = coordinate.Longitude;
        var latitude = coordinate.Latitude;
        var Epsg = coordinate.EPSG;
        
        var attributes = new AttributesTable();
        attributes.Add("Id", id);
        attributes.Add("EPSG", Epsg);
        
        Feature = new Feature(coordinate.GeographyPoint, attributes);
        //FeatureCollection = new FeatureCollection { feature };
        
    }

    
}