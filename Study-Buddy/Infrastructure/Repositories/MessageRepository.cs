using Application.Interfaces.IRepositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(ApplicationContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Message>> GetMessagesByRoomIdAsync(int roomId)
        {
            var room = await _context.Rooms.Include(room => room.Messages)
                                     .ThenInclude(message => message.User)
                                     .FirstOrDefaultAsync(room => room.Id == roomId);
            return room.Messages;
        }
    }
}
