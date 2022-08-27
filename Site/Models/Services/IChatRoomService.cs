using Microsoft.EntityFrameworkCore;
using Site.Context;
using Site.Models.Entites;

namespace Site.Models.Services
{
    public interface IChatRoomService
    {
        Task<Guid>? CreateChatRoom(string ConnectionId);
        Task<Guid> GetChatRoomForConnection(string ConnectionId);
        Task<List<Guid>?> GetAllRoom();
    }

    public class ChatRoomService : IChatRoomService
    {
        private readonly DataBaseContext _context;
        public ChatRoomService(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateChatRoom(string ConnectionId)
        {
            var existChatRoom = _context.ChatRooms?.SingleOrDefault(p => p.ConnectionId == ConnectionId);
            if (existChatRoom != null)
                return await Task.FromResult(existChatRoom.Id);

            ChatRoom chatRoom = new()
            {
                ConnectionId = ConnectionId,
                Id = Guid.NewGuid(),
            };

            _context.ChatRooms?.Add(chatRoom);
            _context.SaveChanges();
            return await Task.FromResult(chatRoom.Id);
        }

        public async Task<List<Guid>?> GetAllRoom()
        {
            var rooms = _context.ChatRooms?
                .Include(p=> p.ChatMessage)
                .Where(p=> p.ChatMessage.Any())
                .Select(p => p.Id).ToList();
            return await Task.FromResult(rooms);
        }

        public async Task<Guid> GetChatRoomForConnection(string ConnectionId)
        {
            var existChatRoom =_context.ChatRooms?.SingleOrDefault(p => p.ConnectionId == ConnectionId);
            return await Task.FromResult(existChatRoom.Id);
        }
    }
}
