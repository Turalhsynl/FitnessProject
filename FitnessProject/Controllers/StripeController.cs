using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitnessProject.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StripeController : ControllerBase
{
    private readonly StripeService _stripeService;

    public StripeController(StripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("create-payment")]
    public IActionResult CreatePayment([FromBody] PaymentRequestDto dto)
    {
        var clientSecret = _stripeService.CreatePayment(dto.TotalPrice, dto.Email);
        return Ok(new { clientSecret });
    }

}
