using System;
using System.Threading.Tasks;

namespace API.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        int Save();
    }
}