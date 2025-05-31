using Microsoft.AspNetCore.Mvc;
using Zadanie10.Exceptions;
using Zadanie10.Services;

namespace Zadanie10.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ClientsController(IDbService service): ControllerBase
{
    [HttpDelete("{idClient}")]
    public async Task<IActionResult> Delete(int idClient)
    {
        try
        {
            await service.DeleteClientAsync(idClient);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}