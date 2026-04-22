using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ms_efcore_sample.classes;
using ms_efcore_sample.classes.Dtos;
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
    public async Task<IActionResult> GetAllJson(){
        try
        {
                
            /*List<FeatureCollection> list = await context.GetAllCoordinateGeojson();
            var json = JsonSerializer.Serialize(list, options);*/
            string json = await context.GetAllCoordinateGeojson();
            var byteCount = Encoding.UTF8.GetByteCount(json);
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
    /// <summary>Denne metoden er laget for å se om all logikken kan være i endepunktkallet</summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    [HttpGet("/amount_geojson")]
    public async Task<IActionResult> GetAmountGeojson(int amount)
    {
        var dbSetList = await context.Coordinates.Take(amount).ToListAsync();
        List<CoordinateGeojsonDto> list = dbSetList.Select(item=>new CoordinateGeojsonDto(item)).ToList();
        var jsonSerializer = new GeojsonSerializer<CoordinateGeojsonDto>(list);
        return Ok(jsonSerializer.Json);
    }

}