namespace _09._IncreaseAgeStoredProcedure
{
    using System;

    public static class Constants
    {

        public const string ClientDB = "MinionsDB";

        public const string InputInvitationText = "Insert your server name please and press Enter!";

        public const string InputMinionId = "Insert Id of minion and press Enter.";

        public const string IncreaseMinionAge = @"EXEC dbo.usp_GetOlder {0}";

        public const string GetData = @"SELECT Name, Age FROM Minions WHERE Id = {0}";

        public const string OutputPattern = "{0} – {1} years old";
        
    }
}
