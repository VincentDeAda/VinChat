namespace ChatApp.Models;

public class EmailConfirmation
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string ConfirmationKey { get; set; } = null!;
    public bool Success { get; set; } = false;
    public DateTime ExpirationDate { get; set; }= DateTime.UtcNow.AddDays(1);

    public Guid IdentityUserId { get; set; } 
    public Identity IdentityUser { get; set; } = null!;
}
