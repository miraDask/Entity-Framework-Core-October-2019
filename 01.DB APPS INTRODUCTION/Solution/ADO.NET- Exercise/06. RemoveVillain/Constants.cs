namespace _06._RemoveVillain
{
    using System;

    public static class Constants
    {

        public const string ClientDB = "MinionsDB";

        public const string InputInvitationText = "Insert your server name please and press Enter!";

        public const string InputVilliantId = "Insert villiant Id and press Enter.";

        public const string GetVilliantNameQuery = @"SELECT Name FROM Villains WHERE Id = {0}";

        public const string DeleteFromMappingtable = @"DELETE FROM MinionsVillains 
                                                        WHERE VillainId = {0}";

        public const string DeleteFromVillains = @"DELETE FROM Villains
                                                    WHERE Id = {0}";

        public const string SuccessMessage = @"{0} was deleted.
{1} minions were released.";
    }
}
