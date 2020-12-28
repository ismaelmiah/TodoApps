using System;
using System.Threading.Tasks;

namespace API.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        Task Save();
    }
}