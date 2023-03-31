using ChatApp.Models;
using Riok.Mapperly.Abstractions;

namespace ChatApp.Utility;
[Mapper]
public partial class Mapper : IMapper
{
    public partial AuthorResponse MapUser(User user);
    [MapProperty(nameof(Message.MessageContent), nameof(MessageResponse.Message))]
    public partial MessageResponse MapMessage(Message message);
    public partial List<MessageResponse> MapMessages(List<Message> messages);
    [MapperIgnoreTarget(nameof(RemovedMessage.Id))] 
    [MapProperty(nameof(Message.Id), nameof(RemovedMessage.MessageId))]
    [MapProperty(nameof(Message.MessageContent), nameof(RemovedMessage.Message))]
    public partial RemovedMessage MapRemovedMessage(Message messages);

    [MapperIgnoreTarget(nameof(MessageEdit.Id))]
    [MapProperty(nameof(Message.Id), nameof(MessageEdit.MessageId))]
    [MapProperty(nameof(Message.MessageContent), nameof(MessageEdit.OldMessage))]
    public partial MessageEdit MapMessageEdit(Message messages);
}
