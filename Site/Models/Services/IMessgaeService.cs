using Site.Context;
using Site.Models.Entites;

namespace Site.Models.Services
{
    public interface IMessgaeService
    {
        Task SaveChatMessage(Guid RoomID, MessageDTO message);
        Task<List<MessageDTO>?> GetChatMessage(Guid RoomID);
    }

    public class MessgaeService : IMessgaeService
    {
        private readonly DataBaseContext _context;
        public MessgaeService(DataBaseContext context)
        {
            _context = context;
        }
        public  Task<List<MessageDTO>?> GetChatMessage(Guid RoomID)
        {
            var messages =  _context?.ChatMessages?.Where(p => p.ChatRoomId == RoomID).Select(p=> new MessageDTO
            {
                Message=p.Message,
                Sender=p.Sender,
                Time=p.Time,
            }).OrderBy(p=> p.Time) .ToList();
            return  Task.FromResult(messages);
        }

        public Task SaveChatMessage(Guid RoomID, MessageDTO message)
        {
            var chatRoom = _context?.ChatRooms?.SingleOrDefault(p => p.Id == RoomID);
            ChatMessage chatMessage   = new()
            {
                ChatRoom = chatRoom ,
                Message = message.Message,
                Sender = message.Sender,
                Time = message.Time,
            };
            _context?.ChatMessages?.Add(chatMessage);
            _context?.SaveChanges();
            return Task.CompletedTask;
        }
    }

    public class MessageDTO
    {
        public string? Sender { get; set; }
        public string? Message { get; set; }
        public DateTime Time { get; set; }
    }
}
