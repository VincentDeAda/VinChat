using System.Text.Json.Serialization;

namespace ChatApp.Models;

public class Message
{
    public Guid Id { get; set; }
    [JsonPropertyName("message")]
    public string MessageContent { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public bool Removed { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;

}
