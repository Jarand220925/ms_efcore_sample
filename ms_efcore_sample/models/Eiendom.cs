using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ms_efcore_sample.models;

public class Eiendom
{
    [Key]
    public int EiendomId { get; set; }
    //public int KommuneId { get; set; }
    //FK blir satt opp automatisk i postgreSQL med navn: "KommuneId"
    public Kommune Kommune { get; set; }
    
    [ForeignKey(nameof(Coordinate))]
    public int? CoordinateId { get; set; }
    public Coordinate Coordinate { get; set; }
    
    public required string ByggType { get; set; }
    
    public required string Adresse { get; set; }
    
    public ICollection<EnergiMerke> EnergiMerker { get; set; }
}