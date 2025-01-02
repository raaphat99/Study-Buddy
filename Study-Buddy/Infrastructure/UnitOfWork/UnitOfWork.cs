using Application.Interfaces.IRepositories;
using Application.Interfaces.UOW;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields
        private readonly ApplicationContext _context;
        private readonly Lazy<IRoomRepository> rooms;
        private readonly Lazy<IMessageRepository> messages;
        private readonly Lazy<ITopicRepository> topics;
        #endregion

        #region Constructor
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            // Lazy<T> class is used to defer the creation of the repositories until they are accessed.
            rooms = new Lazy<IRoomRepository>(() => new RoomRepository(_context));
            messages = new Lazy<IMessageRepository>(() => new MessageRepository(_context));
            topics = new Lazy<ITopicRepository>(() => new TopicRepository(_context));
        }
        #endregion

        #region Getters
        //The Value property of Lazy<T> ensures that the repository is instantiated only once and then reused. (Singleton object)
        public IRoomRepository Rooms => rooms.Value;
        public IMessageRepository Messages => messages.Value;
        public ITopicRepository Topics => topics.Value;
        #endregion

        #region Methods
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
        #endregion

    }
}