namespace API.DataAccessLayer
{
    public class Cassandra
    {
        public string CassandraNodes { get; set; }
        public Cassandraoptions CassandraOptions { get; set; }
    }

    public class Cassandraoptions
    {
        public string KeyspaceName { get; set; }
        public Replication Replication { get; set; }
    }

    public class Replication
    {
        public string Class { get; set; }
        public string replication_factor { get; set; }
    }

}
