using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ms_efcore_sample.models;

public class Eiendom
{
    [Key]
    public int EiendomId { get; set; }
    [ForeignKey("Kommune")]
    public int KommuneId { get; set; }
    [ForeignKey("Coordinate")]
    public int? CoordinateId { get; set; }
    
    public required string ByggType { get; set; }
    
    public required string Adresse { get; set; }
}