namespace _08._IncreaseMinionAge
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(Constants.InputInvitationText);
            var serverName = Console.ReadLine();

            Console.WriteLine(Constants.InputMinionsId);
            var ids = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            var csBuilder = new ConnectionStringBuilder(serverName);
            var connectionString = csBuilder.GetConnectionString(Constants.ClientDB);

            var connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                try
                {
                    SqlCommand command = null;

                    for (int i = 0; i < ids.Length; i++)
                    {
                        command = new SqlCommand(string.Format(Constants.UpdateMinionName, ids[i]), connection);
                        command.ExecuteNonQuery();
                    }

                    command.CommandText = Constants.GetMinionData;
                    var reader = command.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(string.Format(Constants.PrintPattern, reader[0], reader[1]));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
