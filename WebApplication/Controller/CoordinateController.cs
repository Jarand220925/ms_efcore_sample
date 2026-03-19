using Microsoft.AspNetCore.Mvc;
using ms_efcore_sample.models;

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

}