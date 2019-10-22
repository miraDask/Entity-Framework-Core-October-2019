namespace _03._MinionNames
{
    using System;

    public class ConnectionStringBuilder
    {
        public ConnectionStringBuilder(string serverName)
        {
            this.ServerName = serverName;
        }

        public string ServerName { get; set; }

        public string GetConnectionString(string dbName)
        {

            string pattern = "Server={0};" +
                             "Database={1};" +
                             "Integrated Security=true;";
            return String.Format(pattern, this.ServerName, dbName);
        }
    }
}
