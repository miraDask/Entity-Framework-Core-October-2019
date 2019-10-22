namespace _03._MinionNames
{
    using System;

    public static class Constants
    {

        public const string ClientDB = "MinionsDB";

        public const string InputInvitationText = "Insert your server name please and press Enter!";

        public const string InputIdInvitationText = "Insert villiant Id and press Enter.";

        public const string VillianNameQuery = @"SELECT Name FROM Villains WHERE Id = {0}";

        public const string MinionsQuery = @"SELECT ROW_NUMBER() OVER (ORDER BY m.Name) as RowNum,
                                                     m.Name, 
                                                     m.Age
                                                FROM MinionsVillains AS mv
                                                JOIN Minions As m ON mv.MinionId = m.Id
                                               WHERE mv.VillainId = {0}
                                            ORDER BY m.Name";
    }
}
