namespace _04._AddMinion
{
    using System;
    using System.Data.SqlClient;

    public class StartUp
    {
        public static void Main()
        {
            Console.WriteLine(Constants.InputInvitationText);
            var serverName = Console.ReadLine();

            Console.WriteLine(Constants.InputDataInvitationText);
            var minionName = string.Empty;
            var minionAge = 0;
            var minionTown = string.Empty;
            var villianName = string.Empty;

            for (int i = 0; i < 2; i++)
            {
                var data = Console.ReadLine().Split();
                
                if (i == 0)
                {
                    minionName = data[1];
                    minionAge = int.Parse(data[2]);
                    minionTown = data[3];
                }
                else
                {
                    villianName = data[1];
                }
            }

            var csBuilder = new ConnectionStringBuilder(serverName);
            var connectionString = csBuilder.GetConnectionString(Constants.ClientDB);
            var connection = new SqlConnection(connectionString);
            connection.Open();
            SqlTransaction transaction = null;

            using (connection)
            {
                var minionIsAdded = false;
                var villiantIsAdded = false;
                var townIsAdded = false;

                try
                {
                    transaction = connection.BeginTransaction();

                    var command = new SqlCommand(string.Format(Constants.TownCheck, minionTown), connection);
                    command.Transaction = transaction;
                    var townId = command.ExecuteScalar();

                    if(townId == null)
                    {
                        command.CommandText = string.Format(Constants.InsertIntoTowns, minionTown);
                        command.ExecuteNonQuery();
                        townIsAdded = true;
                        
                        command.CommandText = string.Format(Constants.TownCheck, minionTown);
                        townId = command.ExecuteScalar();
                    }

                    command.CommandText = string.Format(Constants.VillianIdQuery, villianName);
                    var vilianId = command.ExecuteScalar();

                    if(vilianId == null)
                    {
                        command.CommandText = string.Format(Constants.InsertIntoVillians, villianName);
                        command.ExecuteNonQuery();
                        villiantIsAdded = true;

                        command.CommandText = string.Format(Constants.VillianIdQuery, villianName);
                        vilianId = command.ExecuteScalar();
                    }

                    command.CommandText = string.Format(Constants.MinionIdQuery, minionName);
                    var minionId = command.ExecuteScalar();

                    if (minionId == null)
                    {
                        command.CommandText = string.Format(Constants.InsertIntoMinions, minionName, minionAge, townId);
                        command.ExecuteNonQuery();
                        minionIsAdded = true;
                        
                        command.CommandText = string.Format(Constants.MinionIdQuery, minionName);
                        minionId = command.ExecuteScalar();
                    }

                    command.CommandText = string.Format(Constants.InsertIntoMappingTable, minionId, vilianId);
                    command.ExecuteNonQuery();

                    transaction.Commit();

                    if(townIsAdded)
                    {
                        Console.WriteLine(string.Format(Constants.TownIsAddedMessage, minionTown));
                    }

                    if (villiantIsAdded)
                    {
                        Console.WriteLine(string.Format(Constants.VillianIsAddedMessage, villianName));
                    }

                    if (minionIsAdded)
                    {
                        Console.WriteLine(string.Format(Constants.MinionIsAddedMessage, minionName));
                    }
                    
                    Console.WriteLine(string.Format(Constants.MinionIsAddedToVillianMessage, minionName, villianName));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                   
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
           
