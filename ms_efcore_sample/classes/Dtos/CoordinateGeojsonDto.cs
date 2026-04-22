using ms_efcore_sample.interfaces;
using ms_efcore_sample.models;
using NetTopologySuite.Features;

namespace ms_efcore_sample.classes.Dtos;

public class CoordinateGeojsonDto : IGeojsonDto
{
    public Feature Feature { get; set; }
    public CoordinateGeojsonDto(Coordinate coordinate)
    {
        var id = coordinate.CoordinateId;
        var pointLatitude = coordinate.GeographyPoint.X;
        var pointLongitude = coordinate.GeographyPoint.Y;
        var Epsg = coordinate.GeographyPoint.SRID;
        
        var attributes = new AttributesTable();
        attributes.Add("Id", id);
        attributes.Add("PointLongitude", pointLongitude);
        attributes.Add("PointLatitude", pointLatitude);
        attributes.Add("EPSG(SRID)", Epsg);
        
        Feature = new Feature(coordinate.GeographyPoint, attributes);
        
    }

    
}