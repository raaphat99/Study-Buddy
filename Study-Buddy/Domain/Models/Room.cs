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
        public DateTime Updated { get; set; } = DateTime.Now;
        public DateTime Created { get; set; } = DateTime.Now;

        [ForeignKey("User")]
        public string? UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Topic")]
        public int? TopicId { get; set; }
        public Topic? Topic { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
