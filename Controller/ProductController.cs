using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Warehouse;

[ApiController]
[Route("/api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;

    public ProductController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById([FromQuery] int id)
    {
        try
        {
            var product = await _service.GetProductByIdAsync(id);
            return Ok(product);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}