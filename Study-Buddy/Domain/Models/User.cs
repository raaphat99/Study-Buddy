using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
