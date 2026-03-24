using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ms_efcore_sample.models;

public class CoordinateDbContext : DbContext
{
    public DbSet<Coordinate> Coordinates { get; set; }
    public DbSet<CoordinateNoPoint> CoordinateNoPoints { get; set; }
    
    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder("Host=localhost;Username=Kodehode;Password=12345;database=Energimerking;");
        dataSourceBuilder.UseNetTopologySuite(); // Configure NTS at the ADO.NET level
        var dataSource = dataSourceBuilder.Build();
        optionsBuilder.UseNpgsql(
            dataSource,
            o => o.UseNetTopologySuite() // Enable NetTopologySuite support
        );
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis"); // Ensure PostGIS extension is created
        modelBuilder.Entity<Coordinate>()
            .Property(e => e.GeographyPoint)
            // Explicitly set the column type to 'geography(Point, 4258)'
            .HasColumnType("geography(Point, 4258)"); 
        // SRID 4326 is standard for global latitude/longitude coordinates
        // Men vi bruker 4258
    }
    
    public async Task<Coordinate> AddCoordinate(int epsg, double latitude, double longitude)
    {
        //Kommentar nedenfor er muligens ikke relevant.
        /* Legg også merke til at _nextId er vekke, samt id constructoren i UserTask. Id håndteringen er nå flyttet til databasen i steden for.  */
        var newCoordinate = new Coordinate
        {
            EPSG =  epsg,
            Latitude = latitude,
            Longitude = longitude
        };
        newCoordinate = newCoordinate.BuildGeographyPoint();
        await Coordinates.AddAsync(newCoordinate);
        await SaveChangesAsync();
        return newCoordinate;
    }
    
    //Kommentar nedenfor er muligens ikke relevant.
    /* I de etterfølgende metodene skal vi jo bare "lese" tasks, vi skal ikke endre de på noen måte. Da kan vi hente ut Tasks.AsNoTracking(), det betyr at vi sier til EF core
    at denne hentingen av data, trenger ingen tracker overhead.  */
    public async Task<List<CoordinateDto>> GetAllCoordinates()
    {
        var dbSetList = await Coordinates.ToListAsync();
        List<CoordinateDto> list = dbSetList.Select(item=>new CoordinateDto(item)).ToList();
        Console.WriteLine($"All coordinateDtos:");
        foreach (var item in list)
        {
            Console.WriteLine(
                $"CoordId: {item.CoordinateId},EPSG: {item.Epsg},Latitude: {item.Latitude}, Longitude: {item.Longitude}");
        }
        Console.WriteLine($"All coordinates:");
        foreach (var item in Coordinates)
        {
            //Krever litt mer for å vise punktdata.
            Console.WriteLine(
                $"CoordId: {item.CoordinateId},EPSG: {item.EPSG},Latitude: {item.Latitude}, Longitude: {item.Longitude} \n" +
                $"GeographyPoint EPSG: {item.GeographyPoint.SRID}, X: {item.GeographyPoint.CoordinateSequence}, Y: {item.GeographyPoint.GeometryType}");
        }
        return list;
    }

    public async Task<List<CoordinateGeojsonDto>> GetAllCoordinateGeojson()
    {
        var dbSetList = await Coordinates.ToListAsync();
        string description = "From running 'name'";
        List<CoordinateGeojsonDto> list = dbSetList.Select(item=>new CoordinateGeojsonDto("GetAllCoordinateGeojson",description,item)).ToList();
        return list;
    }
    
    public async Task<CoordinateNoPoint> AddCoordinateNoPoint(int epsg, double latitude, double longitude)
    {
        //Kommentar nedenfor er muligens ikke relevant.
        /* Legg også merke til at _nextId er vekke, samt id constructoren i UserTask. Id håndteringen er nå flyttet til databasen i steden for.  */
        var newCoordinate = new CoordinateNoPoint()
        {
            EPSG =  epsg,
            Latitude = latitude,
            Longitude = longitude
        };
        await CoordinateNoPoints.AddAsync(newCoordinate);
        await SaveChangesAsync();
        return newCoordinate;
    }
    
    public async Task<List<CoordinateNoPoint>> GetAllCoordinateNoPoints()
    {
        return await CoordinateNoPoints.AsNoTracking().ToListAsync();
    }
    
    
}