namespace _08._IncreaseMinionAge
{
    using System;

    public static class Constants
    {

        public const string ClientDB = "MinionsDB";

        public const string InputInvitationText = "Insert your server name please and press Enter!";

        public const string InputMinionsId = "Insert Ids of minions separated by space.";

        public const string UpdateMinionName = @"UPDATE Minions
                                                   SET Name = UPPER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                                 WHERE Id = {0}";
        
        public const string GetMinionData = @"SELECT Name, Age FROM Minions";

        public const string PrintPattern = "{0} {1}";
    }
}
