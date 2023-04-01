using ChatApp.Models;
using ChatApp.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace ChatApp;

[Authorize]
public class ChatHub : Hub
{

    public static readonly Dictionary<string, string> connectedUsers = new();
    private readonly ChatContext _db;
    private readonly IMapper _mapper;

    public ChatHub(ChatContext db, IMapper mapper) : base()
    {
        _db = db;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        var username = Context.User!.FindFirstValue(ClaimTypes.Name)!;
        connectedUsers.Add(Context.ConnectionId, username);
        await Clients.All.SendAsync("UpdateOnline", connectedUsers.Count);

        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        connectedUsers.Remove(Context.ConnectionId);
        await Clients.All.SendAsync("UpdateOnline", connectedUsers.Count);
        await base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessage(string message)
    {
        var guid = Guid.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var msg = new Message()
        {
            AuthorId = guid,
            MessageContent = message,
        };
        var i = await _db.AddAsync(msg);
        await _db.SaveChangesAsync();
        await i.Reference(x => x.Author).LoadAsync();
        await Clients.All.SendAsync("MessageReceived", _mapper.MapMessage(msg), null, false);
    }

    public async Task RemoveMessage(Guid messageId)
    {
        var caller = Guid.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var message = await _db.FindAsync<Message>(messageId);
        if (message is default(Message) || message.AuthorId != caller)
            return;
        _db.RemovedMessages.Add(_mapper.MapRemovedMessage(message));
        _db.Remove(message);
        await _db.SaveChangesAsync();
        await Clients.All.SendAsync("MessageRemoved", messageId);

    }

    public async Task UpdateMessage(Guid messageId, string newContent)
    {
        var msg = await _db.Messages.Include(x => x.EditHistroy).Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == messageId);
        var authorId = Guid.Parse(Context.User!.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (msg is default(Message) || authorId != msg.AuthorId)
            return;

        var msgEdit = _mapper.MapMessageEdit(msg);
        msg.EditHistroy.Add(msgEdit);
        msg.MessageContent = newContent;
        msg.LastEditDate = DateTime.UtcNow;
        msg.IsEdited= true;
        _db.Update(msg);

        await _db.SaveChangesAsync();
        await Clients.All.SendAsync("MessageUpdated",_mapper.MapMessage(msg));
    }
}
