namespace ms_efcore_sample.models;

public class CoordinateDto
{
    public int CoordinateId { get; set; }
    //SRID
    public int Epsg { get; set; }
    public double Latitude  { get; set; }
    public double Longitude  { get; set; }
    
    public CoordinateDto()
    {
    }

    public CoordinateDto(Coordinate coordinate)
    {
        CoordinateId = coordinate.CoordinateId;
        Epsg = coordinate.GeographyPoint.SRID;
        Latitude = coordinate.GeographyPoint.X;
        Longitude = coordinate.GeographyPoint.Y;
    }
}