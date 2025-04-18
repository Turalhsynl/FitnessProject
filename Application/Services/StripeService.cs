using Microsoft.Extensions.Configuration;
using Stripe;

namespace Application.Services;

public class StripeService
{
    private readonly IConfiguration _configuration;
    public StripeService(IConfiguration configuration)
    {
        _configuration = configuration;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    public string CreatePayment(decimal amount, string email)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100),
            Currency = "usd",
            ReceiptEmail = email
        };

        var service = new PaymentIntentService();
        var paymentIntent = service.Create(options);
        return paymentIntent.ClientSecret;
    }
}
