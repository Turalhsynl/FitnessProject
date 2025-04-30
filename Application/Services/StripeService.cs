using Microsoft.Extensions.Configuration;
using Stripe;

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
            ReceiptEmail = email,
            PaymentMethodTypes = new List<string> { "card" }
        };

        var service = new PaymentIntentService();
        var paymentIntent = service.Create(options);
        return paymentIntent.Id;
    }

    public PaymentIntent ConfirmPayment(string paymentIntentId, string paymentMethodId)
    {
        var service = new PaymentIntentService();
        var options = new PaymentIntentConfirmOptions
        {
            PaymentMethod = paymentMethodId
        };
        var paymentIntent = service.Confirm(paymentIntentId, options);
        return paymentIntent;
    }
}
