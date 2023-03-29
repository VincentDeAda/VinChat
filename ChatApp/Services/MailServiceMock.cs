using ChatApp.Models;

namespace ChatApp.Services;

public class EmailServiceMock : IEmailingService
{
    private readonly ILogger<EmailServiceMock> _logger;

    public EmailServiceMock(ILogger<EmailServiceMock> logger)
    {
        _logger = logger;
    }
    public Task SendConfirmationEmail(EmailConfirmation confirmation)
    {
        _logger.LogInformation($"https://localhost:7117/api/confirmEmail?id={confirmation.Id}&secretKey={confirmation.ConfirmationKey}");

        return Task.FromResult(0);
    }
}
