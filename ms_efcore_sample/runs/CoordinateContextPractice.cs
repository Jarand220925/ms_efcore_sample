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
        await coordinateCtx.AddCoordinate(4258,50.000001,10.000001);
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
}