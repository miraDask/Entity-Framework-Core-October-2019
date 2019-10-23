
namespace _06._RemoveVillain
{
    
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(Constants.InputInvitationText);
            var serverName = Console.ReadLine();

            Console.WriteLine(Constants.InputVilliantId);
            var villianId = Console.ReadLine();

            var csBuilder = new ConnectionStringBuilder(serverName);
            var connectionString = csBuilder.GetConnectionString(Constants.ClientDB);
            var connection = new SqlConnection(connectionString);
            connection.Open();
            SqlTransaction transaction = null;

            using (connection)
            {
                
                try
                {
                    transaction = connection.BeginTransaction();
                    
                    var command = new SqlCommand(string.Format(Constants.GetVilliantNameQuery, villianId), connection);
                    command.Transaction = transaction;
                    var villianName = command.ExecuteScalar();

                    if (villianName == null)
                    {
                        throw new NullReferenceException("No such villain was found.");
                    }

                    command.CommandText = string.Format(Constants.DeleteFromMappingtable, villianId);
                    var rowsAffected = command.ExecuteNonQuery();

                    command.CommandText = string.Format(Constants.DeleteFromVillains, villianId);
                    command.ExecuteNonQuery();

                    transaction.Commit();

                    Console.WriteLine(Constants.SuccessMessage, villianName, rowsAffected);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex.Message);

                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
