using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }

        [ForeignKey("Host")]
        public string HostId { get; set; }
        public virtual ApplicationUser Host { get; set; }

        [ForeignKey("Topic")]
        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
        public virtual ICollection<RoomParticipant> Participants { get; set; } = new List<RoomParticipant>();

        public Room()
        {
            Created = DateTime.UtcNow;
            Updated = DateTime.UtcNow;
        }

    }
}
