using Application.CQRS.Carts.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Kullanıcının sepetini getirir.
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(int userId)
    {
        var result = await _mediator.Send(new GetCart.GetCartQuery { UserId = userId });
        return Ok(result);
    }

    /// <summary>
    /// Sepete ürün ekler.
    /// </summary>
    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCart.AddToCartCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Sepetten ürün çıkarır.
    /// </summary>
    [HttpPost("remove")]
    public async Task<IActionResult> RemoveFromCart([FromBody] RemoveFromCart.RemoveFromCartCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}

