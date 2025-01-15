using Application.DTOs.MessageDTOs;
using Application.DTOs.RoomDTOs;

namespace Presentation.ViewModels
{
    public class ChatRoomViewModel
    {
        public ChatRoomDto? ChatRoomDto { get; set; }
        public MessageCreateDto? MessageCreateDto { get; set; }
        public MessageDeleteDto? MessageDeleteDto { get; set; }

    }
}
