﻿namespace ChatApp.Models;

public class Message
{
    public Guid Id { get; set; }
    public string MessageContent { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.UtcNow;

    public List<MessageEdit> EditHistroy { get; set; } = null!;
    public DateTime? LastEditDate { get;set; } 
    public bool IsEdited { get; set; }
    public Guid AuthorId { get; set; }
    public User Author { get; set; } = null!;

}
