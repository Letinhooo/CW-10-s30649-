using Microsoft.AspNetCore.Mvc;
using Zadanie10.DTOs;
using Zadanie10.Models;
using Zadanie10.Services;

namespace Zadanie10.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TripsController(IDbService service): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetClientTrips([FromQuery] int page=1,[FromQuery] int pageSize=10)
    {
        return Ok(await service.GetTripsDetailsAsync(page, pageSize));
    }

    [HttpPost("{tripId}")]
    public async Task<IActionResult> AddClientTrip([FromRoute] int tripId ,[FromBody] AddClientToTripDto clientTrip)
    {
        await service.AddClientToTripAsync(tripId,clientTrip);
        return Created();
    }
}