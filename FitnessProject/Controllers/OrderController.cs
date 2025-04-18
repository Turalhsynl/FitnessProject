using Application.CQRS.Orders.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Orders.Handlers.GetOrderById;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly ISender _sender;

    public OrderController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrder.CreateOrderCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetOrdersByUserId(int userId)
    {
        var result = await _sender.Send(new GetOrdersByUserId.GetOrdersByUserIdQuery { UserId = userId });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(int orderId)
    {
        var result = await _sender.Send(new GetOrderById.GetOrderByIdQuery(orderId));
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }


    [HttpDelete("{orderId}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var result = await _sender.Send(new DeleteOrder.DeleteOrderCommand { OrderId = orderId });
        return result.IsSuccess ? Ok(result) : NotFound(result);
    }
}
