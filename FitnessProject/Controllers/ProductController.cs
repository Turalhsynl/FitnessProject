using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DeleteProduct;

namespace FitnessProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductController(ISender sender)
        {
            _sender = sender;
        }

        // Endpoint to add a new product (register)
        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] Application.CQRS.Products.Handlers.AddProduct.AddProductCommand request)
        {

            await _sender.Send(request);
            return Ok(new { Message = "Product added successfully." });
        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync([FromQuery] int id)
        {
            var query = new Application.CQRS.Products.Handlers.GetProductById.ProductQuery { Id = id };
            var result = await _sender.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var query = new Application.CQRS.Products.Handlers.GetAllProducts.GetAllProductsQuery();
            var result = await _sender.Send(query);

            if (!result.IsSuccess)
            {
                return NotFound(result.Errors);
            }

            return Ok(result.Data);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] Application.CQRS.Products.Handlers.UpdateProduct.UpdateProductCommand request)
        {
            await _sender.Send(request);
            return Ok(new { Message = "Product updated successfully." });
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteAsync([FromQuery] int id)
        {
            var request = new DeleteProductCommand { Id = id };
            await _sender.Send(request);
            return Ok(new { Message = "Product deleted successfully." });
        }
    }
}
