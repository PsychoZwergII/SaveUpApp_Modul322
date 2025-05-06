using Microsoft.AspNetCore.Mvc;
using SaveUpAppBackend.Models;
using SaveUpAppBackend.Services;

namespace SaveUpAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly MongoDBService _service;

        public ProductsController(MongoDBService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            return Ok(await _service.GetProductsAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Post(Product product)
        {
            product.Date = DateTime.Now;
            return Ok(await _service.CreateProductAsync(product));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)  // id bleibt int
        {
            var success = await _service.DeleteProductAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("clear")]
        public async Task<ActionResult> ClearAll()
        {
            await _service.DeleteAllAsync();
            return NoContent();
        }
    }
}
