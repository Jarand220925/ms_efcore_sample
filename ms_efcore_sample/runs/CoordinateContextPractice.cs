using ms_efcore_sample.models;

namespace ms_efcore_sample.runs;

public class CoordinateContextPractice
{
    public static async Task RunThis()
    {
        using var coordinateCtx = new CoordinateDbContext();

        coordinateCtx.Database.EnsureCreated();

//Vil gi feilmelding om koordinat-id parameteret allerede er på en rad i databasen.
//coordinateCtx.AddCoordinate(1,4258,50.000000,10.000000);
        await coordinateCtx.AddCoordinate(4258,55.000001,14.000001);
        CoordinateDto coordNp = new();

        List<CoordinateDto> getList = new();
        getList = await coordinateCtx.GetAllCoordinates();

        void WriteEmAll()
        {
            Console.WriteLine($"All coordinates:");
            foreach (var item in getList)
            {
                //Krever litt mer for å vise punktdata.
                Console.WriteLine(
                    $"CoordId: {item.CoordinateId},EPSG: {item.Epsg},Latitude: {item.Latitude}, Longitude: {item.Longitude}, \n");
            }
        }

        WriteEmAll();
    }
    
    public static async Task AddNoPoint()
    {
        using var coordinateCtx = new CoordinateDbContext();
        
        coordinateCtx.Database.EnsureCreated();
        await coordinateCtx.AddCoordinateNoPoint(4258,50.000000,10.000000);
    }

    public static async Task CreateAndLook()
    {
        var coord = new Coordinate(70,13)
        {
            CoordinateId = 1,
            EPSG = 3857,
            Latitude = 10.111100,
            Longitude = 50.000000
        };
        Console.WriteLine($"CoordinateId: {coord.CoordinateId}, X: {coord.GeographyPoint.X}, Y: {coord.GeographyPoint.Y} \n" +
                          $"Point: {coord.GeographyPoint.Coordinate}");
    }
}