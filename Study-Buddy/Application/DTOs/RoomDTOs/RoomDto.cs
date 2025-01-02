using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.RoomDTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string HostId { get; set; }
        public string HostUserName { get; set; }
    }
}
