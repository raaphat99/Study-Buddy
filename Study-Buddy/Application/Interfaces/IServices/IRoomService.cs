using Application.DTOs.RoomDTOs;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices
{
    public interface IRoomService
    {
        Task<RoomDto> GetByIdAsync(int id);
        Task<IEnumerable<RoomDto>> GetAllRoomsWithDetailsAsync();
        Task<IEnumerable<RoomDto>> FilterRoomsAsync(string searchQuery);
        Task CreateAsync(RoomCreateDto roomDto);
        Task UpdateAsync(int id, RoomCreateDto roomDto);
        Task DeleteAsync(int id);

    }
}
