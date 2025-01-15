using Application.Interfaces.IRepositories;
using Application.Interfaces.IServices;
using AutoMapper;
using Domain.Models;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs.RoomDTOs;

namespace Application.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomDto> GetByIdAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id);
            var roomDto = new RoomDto
            {
                Id = room.Id,
                HostId = room.HostId,
                Name = room.Name,
                Description = room.Description,
                TopicId = room.TopicId
            };
            return roomDto;
        }

        public async Task<IEnumerable<RoomDto>> GetAllRoomsWithDetailsAsync()
        {
            var rooms = await _roomRepository.GetAllAsync(room => room.Host, room => room.Topic);
            return rooms.Select(room => new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                TopicName = room.Topic.Name,
                HostUserName = room.Host.UserName,
                Created = room.Created,
                Updated = room.Updated
            });
        }

        public async Task<ChatRoomDto> GetChatRoomDetailsAsync(int id)
        {
            var room = await _roomRepository.GetByIdAsync(id, room => room.Host, room => room.Messages, room => room.Participants);
            return new ChatRoomDto
            {
                RoomId = room.Id,
                HostUserName = room.Host.UserName,
                Name = room.Name,
                Description = room.Description,
                Messages = room.Messages.ToList(),
                RoomParticipants = room.Participants.ToList()
            };
        }

        public async Task<IEnumerable<RoomDto>> FilterRoomsAsync(string searchQuery)
        {
            var rooms = await _roomRepository.GetFilteredRoomsAsync(searchQuery);
            return rooms.Select(room => new RoomDto
            {
                Id = room.Id,
                Name = room.Name,
                HostId = room.Host.Id,
                TopicName = room.Topic.Name,
                HostUserName = room.Host.UserName
            });
        }

        public async Task CreateAsync(RoomCreateDto roomDto)
        {
            Room newRoom = new Room
            {
                HostId = roomDto.HostId,
                Name = roomDto.Name,
                Description = roomDto.Description,
                TopicId = roomDto.TopicId
            };

            await _roomRepository.AddAsync(newRoom);
        }

        public async Task UpdateAsync(int id, RoomCreateDto roomDto, string loggedUserId)
        {
            var room = await _roomRepository.GetByIdAsync(id);

            if (room.HostId != loggedUserId)
                throw new UnauthorizedAccessException("You do not have permission to edit this room");

            room.HostId = roomDto.HostId;
            room.Name = roomDto.Name;
            room.Description = roomDto.Description;
            room.TopicId = roomDto.TopicId;

            _roomRepository.Update(room);
        }

        public async Task DeleteAsync(int id, string loggedUserId)
        {
            Room room = await _roomRepository.GetByIdAsync(id);

            if (room.HostId != loggedUserId)
                throw new UnauthorizedAccessException("You do not have permission to delete this room");

            _roomRepository.Remove(room);
        }

    }
}
