using ChatApp.Models;
using System.Net;
using System.Net.Mail;

namespace ChatApp.Services;

public class EmailingService : IEmailingService
{
    private readonly IConfiguration _configuration;

    public EmailingService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendConfirmationEmail(EmailConfirmation confirmation)
    {
        SmtpClient smtpClient = new SmtpClient
        {
            Host = "localhost",
            UseDefaultCredentials = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            EnableSsl = false,
            Credentials = new NetworkCredential("noreply", _configuration["MailUsers:NoReply"])
        };
        MailMessage message = new MailMessage("WhyAreIraqis <noreply@whyareiraqis.com>", confirmation.Email)
        {
            IsBodyHtml = true,
            Body = GenerateMailHTML(confirmation.IdentityUserId, confirmation.ConfirmationKey)
        };
        await smtpClient.SendMailAsync(message);

    }
    private string GenerateMailHTML(Guid userId, string secretKey)
    {
        var html = $"""
            <html>
            <body>
            <a href="https://whyareiraqis.com/api/confirmEmail?id={userId}&secretKey={secretKey}">Confirm</a>
            </body>
            </html>
            """;
        return html;
    }

}
