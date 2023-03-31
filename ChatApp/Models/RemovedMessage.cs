namespace ChatApp.Models;

public class RemovedMessage
{
    public Guid Id { get; set; }
    public Guid MessageId { get; set; }
    public string Message { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public List<MessageEdit> EditHistroy { get; set; } = null!;
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;
    public DateTime DeletionDate { get; set; } = DateTime.UtcNow;
}
