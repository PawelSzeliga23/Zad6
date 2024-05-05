using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Warehouse;
[ApiController]
[Route("/api/[controller]")]
public class WarehouseController : ControllerBase
{
    private IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterProductInWarehouse([FromBody] WarehouseDto dto)
    {
        try
        {
            var idProductWarehouse = await _warehouseService.RegisterProductInWarehouseAsync(dto);
            return Ok(idProductWarehouse);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
    }
    
}