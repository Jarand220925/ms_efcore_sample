namespace ms_efcore_sample.models;

public class CoordinateNoPoint
{
    public int CoordinateNoPointId { get; set; }
    public int EPSG { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}