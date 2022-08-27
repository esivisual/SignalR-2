using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Site.Models.Services;

namespace Site.Hubs
{
    [Authorize]
    public class SupportHub : Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMessgaeService _messgaeService;
        private readonly IHubContext<SiteChatHub> _siteChatHub;
        public SupportHub(IChatRoomService chatRoomService,
            IMessgaeService messgaeService,
            IHubContext<SiteChatHub> siteChatHub)
        {
            _chatRoomService = chatRoomService;
            _messgaeService = messgaeService;
            _siteChatHub = siteChatHub;
        }
        public override async Task OnConnectedAsync()
        {
            var rooms = await _chatRoomService.GetAllRoom();
            await Clients.Caller.SendAsync("GetRooms", rooms);
            await base.OnConnectedAsync();
        }

        public async Task LoadMessage(Guid roomId)
        {
            var message = _messgaeService.GetChatMessage(roomId);
            await Clients.Caller.SendAsync("getNewMessage", message.Result);
        }

        public async Task SendMessage(Guid roomid, string text)
        {
            MessageDTO message = new()
            {
                Message = text,
                Sender = Context?.User?.Identity?.Name,
                Time = DateTime.Now,
            };
            await _messgaeService.SaveChatMessage(roomid, message);
            await _siteChatHub.Clients.Group(roomid.ToString())
                .SendAsync("getNewMessage", message.Sender, message.Message,
                message.Time.ToShortDateString());
        }
    }
}
