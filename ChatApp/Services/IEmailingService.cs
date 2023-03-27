using ChatApp.Models;

namespace ChatApp.Services;

public interface IEmailingService
{
    Task SendConfirmationEmail(EmailConfirmation confirmation);
}
