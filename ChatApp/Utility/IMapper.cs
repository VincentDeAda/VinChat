using ChatApp.Models;

namespace ChatApp.Utility;
public interface IMapper
{
    AuthorResponse MapUser(User user);
    RemovedMessage MapRemovedMessage(Message messages);
    MessageResponse MapMessage(Message message);
    List<MessageResponse> MapMessages(List<Message> messages);
    MessageEdit MapMessageEdit(Message messages);
}