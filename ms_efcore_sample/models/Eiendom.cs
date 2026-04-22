namespace ms_efcore_sample.models;

public class Eiendom
{
    public int EiendomId { get; set; }
    
    public int KommuneId { get; set; }
    
    public int CoordinateId { get; set; }
    
    public required string Type { get; set; }
}