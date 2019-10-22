namespace _02._VillainNames
{
    
    using System;
    using System.Data.SqlClient;
    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(Constants.InputInvitationText);
            var serverName = Console.ReadLine();

            var csBuilder = new ConnectionStringBuilder(serverName);
            var connectionString = csBuilder.GetConnectionString(Constants.ClientDB);
            var connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                try
                {
                    var command = new SqlCommand(Constants.QueryText, connection);
                    var reader = command.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} - {reader[1]}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                   
                }
            }
        }
    }
}
