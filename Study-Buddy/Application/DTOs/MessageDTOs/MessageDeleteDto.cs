using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.MessageDTOs
{
    public class MessageDeleteDto
    {
        public string? UserId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public int MessageId { get; set; }
    }
}
