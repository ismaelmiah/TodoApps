using System;
using API.DataAccessLayer;
using Cassandra;
using Cassandra.Mapping.Attributes;

namespace API.Foundation.Entities
{
    public class TodoItem : IEntity<int>
    {
        [Column("id")]
        [PartitionKey()]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("datetime", Type = typeof(LocalDate))]
        public DateTime DateTime { get; set; }
    }
}
