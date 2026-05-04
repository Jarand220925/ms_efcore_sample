using Microsoft.EntityFrameworkCore;
using ms_efcore_sample.classes;
using ms_efcore_sample.classes.Dtos;
using NetTopologySuite.Features;
using Npgsql;

namespace ms_efcore_sample.models;

public class CoordinateDbContext : DbContext
{
    public DbSet<Coordinate> Coordinates { get; set; }
    public DbSet<CoordinateNoPoint> CoordinateNoPoints { get; set; }
    
    public DbSet<Kommune> Kommuner { get; set; }
    
    public DbSet<Eiendom> Eiendommer { get; set; }
    
    

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
    
    public async Task<Eiendom> AddEiendom(string adresse, int kommuneId, int coordinateId, string byggType)
    {
        var newEiendom = new Eiendom
        {
            Adresse = adresse,
            //KommuneId = kommuneId,
            CoordinateId = coordinateId,
            ByggType = byggType
        };
        await Eiendommer.AddAsync(newEiendom);
        await SaveChangesAsync();
        return newEiendom;
    }
    
    public async Task<Kommune> AddKommune(string navn)
    {
        var newKommune = new Kommune
        {
            Navn = navn
        };
        await Kommuner.AddAsync(newKommune);
        await SaveChangesAsync();
        return newKommune;
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
    //Gammel
    /*public async Task<List<FeatureCollection>> GetAllCoordinateGeojson()
    {
        var dbSetList = await Coordinates.ToListAsync();
        string description = "From running 'name'";
        List<CoordinateGeojsonDto> list = dbSetList.Select(item=>new CoordinateGeojsonDto("GetAllCoordinateGeojson",description,item)).ToList();
        return list.Select(item => item.FeatureCollection).ToList();
    }*/
    
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
        //Bruker denne funksjonen til å teste datainnsamling.
        
        //Henter eiendommer med energimerker og kommuner.
        var somedata = await Eiendommer.Include(e => e.EnergiMerker)
            .Include(e => e.Kommune).ToListAsync();
        //Legger kommunene i egen liste.
        var kommuner = somedata.Select(s => s.Kommune).ToList();
        //Legger energimerkene i egen liste.
        var energiMrk = somedata.Select(e => e.EnergiMerker);
        //zzz
        //Denne linjen gjør det endepunktet er ment for.
        return await CoordinateNoPoints.AsNoTracking().ToListAsync();
    }
    /// <summary>
    /// Finner eiendommer som referer til ett koordinat som
    /// igjen blir referert til av en eller flere andre eiendommer.
    /// </summary>
    /// <returns>Filtrert liste med eiendommer</returns>
    public async Task<List<Eiendom>> GetAllEiendommerWithManyGeos()
    {
        var coordinates = await Coordinates.AsNoTracking().ToListAsync();
        List<Eiendom> eiendomsListe = new();
        foreach (var item in coordinates)
        {
            var eiendom = await Eiendommer.Where(e => e.CoordinateId == item.CoordinateId).ToListAsync();
            if (eiendom != null)
            {
                eiendomsListe.AddRange(eiendom);
            }
        }
        List<Eiendom> filtrertEiendomsListe = eiendomsListe.GroupBy(e => e.CoordinateId)
            .Where(group => group.Count() > 1)
            .SelectMany(group => group)
            .ToList();
        return filtrertEiendomsListe;
    }
    /// <summary>
    /// Simpel utspørring etter eiendommer på ett koordinat
    /// </summary>
    /// <param name="coordinateId"></param>
    /// <returns>List of Eiendom</returns>
    public async Task<Object> GetEiendommerOnPoint(int coordinateId)
    {
        var coordinate = Coordinates.FindAsync(coordinateId).Result;
        var eiendomsListe = await Eiendommer
            .Where(e => e.CoordinateId == coordinate.CoordinateId).ToListAsync(); 
        return eiendomsListe;
    }
    
    /// <summary>
    /// Looks at every coordinate, filtering out any that doesn't have "Kommunenummer" or Geography.
    /// Is prone to crashing web-browser.
    /// Result will likely exceed 40mb.
    /// </summary>
    /// <returns>Gives a serialized string of all coordinates</returns>
    public async Task<string> GetAllCoordinateGeojson()
    {
        var dbSetList = await Coordinates.ToListAsync();
        List<CoordinateGeojsonDto> list = dbSetList.Select(item=>new CoordinateGeojsonDto(item)).ToList();
        var jsonSerializer = new GeojsonSerializer<CoordinateGeojsonDto>(list);
        return jsonSerializer.Json;
    }
    
    public async Task<string> GetAmountCoordinateGeojson(int amount)
    {
        var dbSetList = await Coordinates.Take(amount).ToListAsync();
        List<CoordinateGeojsonDto> list = dbSetList.Select(item=>new CoordinateGeojsonDto(item)).ToList();
        var jsonSerializer = new GeojsonSerializer<CoordinateGeojsonDto>(list);
        return jsonSerializer.Json;
    }
    
    
}