using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.RoomDTOs
{
    public class RoomCreateDto
    {
        public int? Id { get; set; }

        [Required]
        public string HostId { get; set; }

        [StringLength(50, ErrorMessage = "Room Name is too long.")]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public int TopicId { get; set; }
    }
}
