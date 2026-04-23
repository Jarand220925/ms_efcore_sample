using System.ComponentModel.DataAnnotations;

namespace ms_efcore_sample.models;

public class Kommune
{
    [Key]
    public int KommuneId {get; set;}
    public string Navn { get; set; }
}