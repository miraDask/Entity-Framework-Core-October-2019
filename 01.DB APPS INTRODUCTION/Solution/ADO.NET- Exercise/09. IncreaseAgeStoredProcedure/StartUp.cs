namespace _09._IncreaseAgeStoredProcedure
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(Constants.InputInvitationText);
            var serverName = Console.ReadLine();

            Console.WriteLine(Constants.InputMinionId);
            var id = Console.ReadLine();

            var csBuilder = new ConnectionStringBuilder(serverName);
            var connectionString = csBuilder.GetConnectionString(Constants.ClientDB);

            var connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                try
                {
                    var command = new SqlCommand(string.Format(Constants.IncreaseMinionAge, id), connection);
                    command.ExecuteNonQuery();

                    command.CommandText = string.Format(Constants.GetData, id);
                    var reader = command.ExecuteReader();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine(string.Format(Constants.OutputPattern, reader[0], reader[1]));

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
