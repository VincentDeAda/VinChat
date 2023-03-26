namespace ChatApp.Models;

public class Identity 
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public bool EmailConfirmed { get; set; } = false;
    
    public List<EmailConfirmation> EmailsConfirmed { get; set; }=null!;
    public User User { get; set; } = null!;


}
