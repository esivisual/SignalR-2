using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Site.Models.Services;

namespace Site.Hubs
{
    public class SiteChatHub : Hub
    {
        private readonly IChatRoomService  _chatRoomService;
        private readonly IMessgaeService _messgaeService;
        public SiteChatHub(IChatRoomService chatRoomService, IMessgaeService messgaeService)
        {
            _chatRoomService = chatRoomService;
            _messgaeService = messgaeService;
        }

        public async Task SendNewMessage(string Sender, string Message)
        {
            var roomId =await _chatRoomService.GetChatRoomForConnection(Context.ConnectionId);
            MessageDTO messageDTO = new()
            {
                Sender = Sender,
                Message = Message,
                Time = DateTime.Now,
            };
            await _messgaeService.SaveChatMessage(roomId, messageDTO);

            await Clients.Groups(roomId.ToString()).SendAsync("getNewMessage", messageDTO.Sender, messageDTO.Message, messageDTO.Time);
        }

        [Authorize]
        public async Task JoinRoom(Guid roomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        }
        
        [Authorize]
        public async Task LeaveRoom(Guid roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                await base.OnConnectedAsync();
                return;
            }
            var roomId = await _chatRoomService.CreateChatRoom(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
            await Clients.Caller.SendAsync("getNewMessage", "پشتیبانی سایت شما", "سلام وقت بخیر - چطور میتونم کمکتون کنم ؟");
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
