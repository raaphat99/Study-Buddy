using Application.DTOs.MessageDTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RoomDTOs
{
    public class ChatRoomDto
    {
        public int RoomId { get; set; }
        public string HostUserName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<RoomParticipant> RoomParticipants { get; set; }
        public MessageCreateDto MessageCreateDto { get; set; }
    }
}
