using Application.Interfaces.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        IDeveloperRepository Developers { get; }
        Task<int> CompleteAsync();
    }
}
