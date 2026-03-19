using Microsoft.EntityFrameworkCore;

namespace ms_efcore_sample.models;

public class BlogContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    /// <summary>
    /// <b>Ikke i bruk</b>
    /// </summary>
    public string DbPath { get; }

    public BlogContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform. BUT, it requires a connection to a database rather if using
    // PostgreSQL.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseNpgsql($"Host=localhost;Username=Kodehode;Password=12345;database=Energimerking;");
}