namespace _03._MinionNames
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(Constants.InputInvitationText);
            var serverName = Console.ReadLine();

            Console.WriteLine(Constants.InputIdInvitationText);
            var villiantId = Console.ReadLine();

            var csBuilder = new ConnectionStringBuilder(serverName);
            var connectionString = csBuilder.GetConnectionString(Constants.ClientDB);
            var connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                try
                {
                   
                    var nameQueryText = String.Format(Constants.VillianNameQuery, villiantId);
                    var minionsQueryText = String.Format(Constants.MinionsQuery, villiantId);

                    var command = new SqlCommand(nameQueryText, connection);
                    var villianName = command.ExecuteScalar();

                    if (villianName == null)
                    {
                        Console.WriteLine($"No villain with ID {villiantId} exists in the database.");
                        return;
                    }

                    Console.WriteLine($"Villain: {villianName}");

                    command = new SqlCommand(minionsQueryText, connection);
                    var reader = command.ExecuteReader();

                    using (reader)
                    {
                        int count = 0;

                        while (reader.Read())
                        {
                            var row = reader[0];
                            var name = reader[1];
                            var age = reader[2];

                            Console.WriteLine($"{row}. {name} {age}");
                            count++;
                        }

                        if (count == 0)
                        {
                            Console.WriteLine("(no minions)");
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
