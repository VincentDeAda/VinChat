using System.Text.Json.Serialization;

namespace ChatApp.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    [JsonIgnore]
    public string HashedPassword { get; set; } = null!;
    [JsonIgnore]
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    [JsonIgnore]
    public string Salt { get; set; } = null!;

    [JsonIgnore]
    public bool IsBanned { get; set; }
    public List<Message> Messages { get; set; } = new List<Message>();

}
