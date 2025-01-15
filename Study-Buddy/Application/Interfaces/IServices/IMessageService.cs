using Application.DTOs.MessageDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetAllMessagesAsync(int roomId);
        Task CreateAsync(MessageCreateDto messageDto);
        Task UpdateAsync(int id, MessageCreateDto messageDto, string loggedUserId);
        Task DeleteAsync(int id, string loggedUserId);
    }
}
