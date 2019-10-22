namespace _01._Initial_Setup
{
    using System;
    using System.Data.SqlClient;
    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(Constants.InputInvitationText);

            string serverName = Console.ReadLine();

            ConnectionStringBuilder csBuilder = new ConnectionStringBuilder(serverName);
            string connectionString = csBuilder.GetConnectionString(Constants.MasterDB);

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            using (connection)
            {
                try
                {
                    string queryText = String.Format(Constants.CreateDBText, Constants.ClientDB);
                    SqlCommand createDB = new SqlCommand(queryText, connection);
                    createDB.ExecuteNonQuery();
                    Console.WriteLine(Constants.DatabaseCreatedMessage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

           
            connectionString = csBuilder.GetConnectionString(Constants.ClientDB);
            connection = new SqlConnection(connectionString);
            connection.Open();

            using (connection)
            {
                
                try
                {
                    SqlCommand createTables = new SqlCommand(Constants.CreateQueryText, connection);
                    createTables.ExecuteNonQuery();
                    Console.WriteLine(Constants.TablesCreatedMessage);

                    SqlCommand insertCommand = new SqlCommand(Constants.InsertQueryText, connection);
                    int affectedRows = insertCommand.ExecuteNonQuery();
                    Console.WriteLine(Constants.AffectedRowsMessage, affectedRows);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
