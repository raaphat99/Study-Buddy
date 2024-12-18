using Application.Interfaces.IRepositories;
using Application.Interfaces.UOW;
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
        private readonly Lazy<IDeveloperRepository> developers;
        #endregion

        #region Constructor
        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            // Lazy<T> class is used to defer the creation of the repositories until they are accessed.
            developers = new Lazy<IDeveloperRepository>(() => new DeveloperRepository(_context));
        }
        #endregion

        #region Getters
        //The Value property of Lazy<T> ensures that the repository is instantiated only once and then reused. (Singleton object)
        public IDeveloperRepository Developers => developers.Value;
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