using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional custom properties specific to the application can go here
        public virtual ICollection<RoomParticipant> Rooms { get; set; } = new List<RoomParticipant>();
        public virtual ICollection<Room> HostedRooms { get; set; } = new List<Room>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
