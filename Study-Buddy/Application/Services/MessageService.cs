using Application.DTOs.MessageDTOs;
using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<MessageDto>> GetAllMessagesAsync(int roomId)
        {
            var messages = await _messageRepository.GetMessagesByRoomIdAsync(roomId);
            var messageDtos = new List<MessageDto>();

            foreach (var message in messages)
            {
                messageDtos.Add(new MessageDto()
                {
                    Sender = message.User.UserName,
                    Body = message.Body,
                    Created = message.Created,
                    Updated = message.Updated
                });
            }
            return messageDtos;
        }

        public async Task CreateAsync(MessageCreateDto messageDto)
        {
            var message = new Message()
            {
                UserId = messageDto.UserId,
                RoomId = messageDto.RoomId,
                Body = messageDto.Body,
            };
            await _messageRepository.AddAsync(message);
        }

        public async Task UpdateAsync(int id, MessageCreateDto messageDto, string loggedUserId)
        {
            Message message = await _messageRepository.GetByIdAsync(id);

            if (message.UserId != loggedUserId)
                throw new UnauthorizedAccessException("You don't have permission to edit this message!");

            message.Body = messageDto.Body;
            _messageRepository.Update(message);
        }

        public async Task DeleteAsync(int id, string loggedUserId)
        {
            Message message = await _messageRepository.GetByIdAsync(id);

            if (message.UserId != loggedUserId)
                throw new UnauthorizedAccessException("You don't have permission to delete this message!");

            _messageRepository.Remove(message);
        }
    }
}
