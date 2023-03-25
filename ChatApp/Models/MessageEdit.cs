namespace ChatApp.Models;

public class MessageEdit
{
    public Guid Id { get; set; }
    public DateTime EditDate { get; set; } = DateTime.UtcNow;
    public string OldMessage { get; set; } = null!;


    public Guid MessageId { get; set; }
    public Message Message { get; set; }=null!;
}
