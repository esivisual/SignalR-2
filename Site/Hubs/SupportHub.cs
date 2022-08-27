using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Site.Models.Services;

namespace Site.Hubs
{
    [Authorize]
    public class SupportHub:Hub
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IMessgaeService messgaeService;
        public SupportHub(IChatRoomService chatRoomService, IMessgaeService messgaeService)
        {
            _chatRoomService = chatRoomService;
            this.messgaeService = messgaeService;   
        }
        public override async Task OnConnectedAsync()
        {
            var rooms = await _chatRoomService.GetAllRoom();
            await Clients.Caller.SendAsync("GetRooms",rooms);
            await base.OnConnectedAsync();
        }

        public async Task LoadMessage(Guid roomId)
        {
            var message= messgaeService.GetChatMessage(roomId);
            await Clients.Caller.SendAsync("getNewMessage", message.Result);
        }
    }
}
