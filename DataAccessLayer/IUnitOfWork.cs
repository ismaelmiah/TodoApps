using System;

namespace API.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}