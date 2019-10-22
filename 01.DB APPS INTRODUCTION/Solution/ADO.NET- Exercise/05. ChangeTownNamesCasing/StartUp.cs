
namespace _05._ChangeTownNamesCasing
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(Constants.InputInvitationText);
            var serverName = Console.ReadLine();

            Console.WriteLine(Constants.InputCountryName);
            var countryName = Console.ReadLine();

            var csBuilder = new ConnectionStringBuilder(serverName);
            var connectionString = csBuilder.GetConnectionString(Constants.ClientDB);

            var connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                try
                {
                    var command = new SqlCommand(string.Format(Constants.UpdateQuery, countryName), connection);
                    var rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        Console.WriteLine(Constants.NonTownsAffected);
                        return;
                    }

                    Console.WriteLine(string.Format(Constants.AffectedRows, rowsAffected));

                    command.CommandText = string.Format(Constants.TownNamesQuety, countryName);
                    var reader = command.ExecuteReader();
                    var towns = new List<string>();

                    using (reader)
                    {
                        while (reader.Read())
                        {
                            towns.Add(reader[0].ToString());
                        }
                    }

                    Console.WriteLine($"[{string.Join(", ", towns)}]");
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
