using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using ms_efcore_sample.models;
using NetTopologySuite.Features;
using NetTopologySuite.IO.Converters;

namespace WebApplication.Controller;

[ApiController]
[Route("/[Controller]")]
public class CoordinateController(CoordinateDbContext context, ILogger<CoordinateDbContext> logger) : ControllerBase
{
    // [HttpGet]
    // /* Det kan være lurt å matche navnet på metoden i controlleren, med navnet på http metode + subroute. */
    // public async Task<IActionResult> Get([FromQuery] QueryDto queryDto) 
    // {
    //     try 
    //     {
    //         return Ok(await queryDto.BuildQuery(context));
    //     }
    //     catch (Exception ex)
    //     {
    //         logger.LogError(ex.Message);
    //         return StatusCode(500, ex.Message);
    //     }
    // }
    
    [HttpGet("/emall")]
    /* Se navngivningen på denne metoden. */
    public async Task<IActionResult> GetEmAll(){
        try
        {
            return Ok(await context.GetAllCoordinates());
        }
        catch(Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("/alljson")]
    /* Se navngivningen på denne metoden. */
    public async Task<IActionResult> GetAllJson(){
        try
        {
            var options = new JsonSerializerOptions
            {
                Converters = { new GeoJsonConverterFactory() },
                // Optional: for pretty formatting
                WriteIndented = true 
            };
            List<FeatureCollection> list = await context.GetAllCoordinateGeojson();
            var json = JsonSerializer.Serialize(list, options);
            return Ok(json);
            
        }
        catch(Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }
    
    [HttpGet("/allnopoints")]
    public async Task<IActionResult> GetAllNoPoints(){
        try
        {
            return Ok(await context.GetAllCoordinateNoPoints());
        }
        catch(Exception ex)
        {
            logger.LogError(ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

}