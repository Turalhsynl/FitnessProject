using Application.CQRS.Orders.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Application.CQRS.Orders.Handlers.CreateOrder;
using static Application.CQRS.Orders.Handlers.GetOrderById;
using static Application.CQRS.Orders.Handlers.GetOrderLines;

namespace FitnessProject.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
    {
        var result = await _sender.Send(command);

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return BadRequest();
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var result = await _sender.Send(new GetOrderQuery(orderId));

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return NotFound();
    }

    [HttpGet("{orderId}/orderlines")]
    public async Task<IActionResult> GetOrderLines(int orderId)
    {
        var result = await _sender.Send(new GetOrderLinesQuery(orderId));

        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }

        return NotFound();
    }
}

