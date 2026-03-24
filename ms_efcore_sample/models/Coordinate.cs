using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace ms_efcore_sample.models;

public class Coordinate
{
    public int CoordinateId { get; set; }
    public int EPSG { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public Point GeographyPoint { get; set; }

    public Coordinate()
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4258);
        GeographyPoint = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(Latitude,Longitude));
    }
    
    public Coordinate(double latitude, double longitude)
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4258);
        GeographyPoint = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(latitude,longitude));
    }

    public Coordinate BuildGeographyPoint()
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4258);
        GeographyPoint = geometryFactory.CreatePoint(new NetTopologySuite.Geometries.Coordinate(Latitude,Longitude));
        return this;
    }
}