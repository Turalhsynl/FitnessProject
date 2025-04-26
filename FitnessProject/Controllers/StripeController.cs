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

    // Ödəniş yaratmaq
    [HttpPost("create-payment")]
    public IActionResult CreatePayment([FromBody] PaymentRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email) || request.Amount <= 0)
        {
            return BadRequest("Invalid payment request.");
        }

        var paymentIntentId = _stripeService.CreatePayment(request.Amount, request.Email);
        return Ok(new { PaymentIntentId = paymentIntentId });
    }

    // PaymentIntent təsdiqləmək
    [HttpPost("confirm-payment")]
    public IActionResult ConfirmPayment([FromBody] ConfirmPaymentRequest request)
    {
        if (string.IsNullOrEmpty(request.PaymentIntentId) || string.IsNullOrEmpty(request.PaymentMethodId))
        {
            return BadRequest("PaymentIntentId and PaymentMethodId are required.");
        }

        var paymentIntent = _stripeService.ConfirmPayment(request.PaymentIntentId, request.PaymentMethodId);
        if (paymentIntent.Status == "succeeded")
        {
            return Ok(new { Status = "Payment successful" });
        }

        return BadRequest(new { Status = "Payment failed" });
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; } // Ödəniş məbləği
        public string Email { get; set; }  // İstifadəçi e-poçtu
    }

    // PaymentIntent təsdiqləmək üçün lazım olan model
    public class ConfirmPaymentRequest
    {
        public string PaymentIntentId { get; set; }
        public string PaymentMethodId { get; set; }
    }

}
