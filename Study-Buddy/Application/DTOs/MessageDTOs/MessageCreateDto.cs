using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.MessageDTOs
{
    public class MessageCreateDto
    {
        public string? UserId { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Message content cannot exceed 500 characters.")]
        public string Body { get; set; }
    }
}
