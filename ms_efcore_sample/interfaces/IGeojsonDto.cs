using NetTopologySuite.Features;

namespace ms_efcore_sample.interfaces;
/// <summary>
/// Lagrer Feature-en som skal brukes i GeojsonSerializer.
/// </summary>
public interface IGeojsonDto
{
    public Feature Feature { get; set; }
}