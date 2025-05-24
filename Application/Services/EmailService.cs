using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net;

namespace Application.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpHost = _configuration["Smtp:Host"];
        var smtpPort = int.Parse(_configuration["Smtp:Port"]);
        var smtpUser = _configuration["Smtp:Username"];
        var smtpPass = _configuration["Smtp:Password"];
        var fromEmail = _configuration["Smtp:From"];

        var message = new MailMessage
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };

        try
        {
            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "an ocurred Error while sending email");
            throw;
        }
    }
}
