namespace ms_efcore_sample.models;

public class CoordinateDto
{
    public int CoordinateId;
    //SRID
    public int Epsg;
    public double Latitude;
    public double Longitude;
    
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