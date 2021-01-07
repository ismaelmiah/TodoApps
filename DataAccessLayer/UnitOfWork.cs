using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace API.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;
        public UnitOfWork(DbContext context)
        {
            _context = context;
        }
        public void Dispose()
        {
            _context.Dispose();
        }

        public int  Save()
        {
            return _context.SaveChanges();
        }
    }
}
