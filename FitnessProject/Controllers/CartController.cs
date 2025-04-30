using Application.CQRS.Carts.ResponseDto;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Common;
using System.Security.Claims;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CartController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateCart([FromBody] CreateCartCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("add-product")]
    public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Errors);
    }

    [HttpGet("{cartId}")]
    public async Task<IActionResult> GetCart(int cartId)
    {
        var result = await _sender.Send(new GetCartQuery { CartId = cartId });
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Errors);
    }

    [HttpDelete("remove-product/{cartId}/{productId}")]
    public async Task<IActionResult> DeleteProductFromCart(int cartId, int productId)
    {
        var result = await _sender.Send(new DeleteProductFromCartCommand { CartId = cartId, ProductId = productId });
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        return BadRequest(result.Errors);
    }

    [HttpGet("get/{userId}")]
    public async Task<IActionResult> GetCartByUserId(int userId)
    {
        var result = await _sender.Send(new GetCartByUserIdQuery { UserId = userId });

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Data);
    }
}
