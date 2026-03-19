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
        GeographyPoint = new Point(Latitude, Longitude);
        GeographyPoint.SRID = 4258;
    }
}