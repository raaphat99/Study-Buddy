using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.MessageDTOs
{
    public class MessageDto
    {
        public string Sender { get; set; }
        public string Body { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }
    }
}
