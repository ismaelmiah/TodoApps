using API.Foundation.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Foundation.Contexts
{
    public interface ITodoContext
    {
        DbSet<TodoItem> TodoItems { get; set; }
    }
}