using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RoomParticipant
    {
        [ForeignKey("Room")]
        public int RoomId { get; set; }
        public virtual Room Room { get; set; }

        [ForeignKey("Participant")]
        public string ParticipantId { get; set; }
        public virtual ApplicationUser Participant { get; set; }
    }
}
