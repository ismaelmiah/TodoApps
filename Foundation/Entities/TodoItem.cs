using System;
using API.DataAccessLayer;

namespace API.Foundation.Entities
{
    public class TodoItem : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime DateTime { get; set; }
    }
}
