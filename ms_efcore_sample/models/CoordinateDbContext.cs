using Microsoft.EntityFrameworkCore;

namespace ms_efcore_sample.models;

public class CoordinateDbContext : DbContext
{
    public DbSet<Coordinate> Coordinates { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(
            "Host=localhost;Username=Kodehode;Password=12345;database=Energimerking;", 
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
        var newCoordinate = new Coordinate()
        {
            EPSG =  epsg,
            Latitude = latitude,
            Longitude = longitude
        };
        await Coordinates.AddAsync(newCoordinate);
        await SaveChangesAsync();
        return newCoordinate;
    }
    
    //Kommentar nedenfor er muligens ikke relevant.
    /* I de etterfølgende metodene skal vi jo bare "lese" tasks, vi skal ikke endre de på noen måte. Da kan vi hente ut Tasks.AsNoTracking(), det betyr at vi sier til EF core
    at denne hentingen av data, trenger ingen tracker overhead.  */
    public async Task<List<Coordinate>> GetAllCoordinates()
    {
        return await Coordinates.AsNoTracking().ToListAsync();
    }
}