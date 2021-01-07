using System;
using System.ComponentModel.DataAnnotations;
using API.DataAccessLayer;
using Cassandra;
using Cassandra.Mapping.Attributes;

namespace API.Foundation.Entities
{
    public class TodoItem : IEntity<int>
    {
        [Column("id")]
        [PartitionKey(0)]
        public int Id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("datetime")]
        
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
    }
}
