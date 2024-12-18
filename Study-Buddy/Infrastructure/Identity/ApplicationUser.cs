using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Additional custom properties specific to the application can go here
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
