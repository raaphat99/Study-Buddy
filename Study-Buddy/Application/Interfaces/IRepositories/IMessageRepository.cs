﻿using Application.DTOs.RoomDTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IRepositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        public Task<IEnumerable<Message>> GetMessagesByRoomIdAsync(int roomId);
    }
}
