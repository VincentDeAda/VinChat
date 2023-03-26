namespace ChatApp.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public Guid IdentityId { get; set; } 
    public Identity Identity { get; set; } = null!;
    public List<Message> Messages { get; set; } = new List<Message>();

}
