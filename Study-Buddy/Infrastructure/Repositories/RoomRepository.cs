using Domain.Models;
using Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Data;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.UOW;

namespace Infrastructure.Repositories
{
    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {
        public RoomRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Room>> GetAllAsync(params Expression<Func<Room, object>>[] includes)
        {
            IQueryable<Room> query = _dbSet;

            if (includes != null && includes.Length != 0)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query
                .AsNoTracking()
                .OrderByDescending(room => room.Updated)
                .ThenByDescending(room => room.Created)
                .ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetFilteredRoomsAsync(string searchQuery)
        {
            IQueryable<Room> query = _dbSet;

            query.Include(room => room.User)
                 .Include(room => room.Topic);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(room =>
                            room.Name.Contains(searchQuery) ||
                            room.User.UserName.Contains(searchQuery) ||
                            room.Topic.Name.Contains(searchQuery));
            }

            return await query.AsNoTracking().ToListAsync();
        }
    }
}
