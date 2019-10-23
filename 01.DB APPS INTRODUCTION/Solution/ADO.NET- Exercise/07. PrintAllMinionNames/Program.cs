namespace _07._PrintAllMinionNames
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    public class Program
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

                    var command = new SqlCommand(Constants.GetMinionsNames, connection);
                    var reader = command.ExecuteReader();
                    var minionsList = new List<string>();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            minionsList.Add(reader[0].ToString());
                        }
                    }

                    var halfMinionsCount = minionsList.Count / 2;

                    for (int i = 0; i < halfMinionsCount; i++)
                    {
                        Console.WriteLine(minionsList[i]);

                        Console.WriteLine(minionsList[minionsList.Count - 1 - i]);
                    }

                    if (minionsList.Count % 2 == 1)
                    {
                        Console.WriteLine(minionsList[halfMinionsCount]);
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
