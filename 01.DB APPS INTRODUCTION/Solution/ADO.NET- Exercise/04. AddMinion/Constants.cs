namespace _04._AddMinion
{
    using System;

    public static class Constants
    {

        public const string ClientDB = "MinionsDB";

        public const string InputInvitationText = "Insert your server name please and press Enter!";

        public const string InputDataInvitationText = "Insert information about minion and villian and press Enter.";

        public const string VillianIdQuery = @"SELECT Id FROM Villains WHERE Name = '{0}'";

        public const string MinionIdQuery = @"SELECT Id FROM Minions WHERE Name = '{0}'";

        public const string InsertIntoMappingTable = @"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES ({0}, {1})";

        public const string InsertIntoVillians = @"INSERT INTO Villains (Name, EvilnessFactorId)  VALUES ('{0}', 4)";

        public const string InsertIntoMinions = @"INSERT INTO Minions (Name, Age, TownId) VALUES ('{0}', {1}, {2})";

        public const string InsertIntoTowns = @"INSERT INTO Towns (Name) VALUES ('{0}')";

        public const string TownCheck = @"SELECT Id FROM Towns WHERE Name = '{0}'";

        public const string TownIsAddedMessage = @"Town {0} was added to the database.";

        public const string VillianIsAddedMessage = @"Villiant {0} was added to the database.";

        public const string MinionIsAddedMessage = @"Minion {0} was added to the database.";

        public const string MinionIsAddedToVillianMessage = @"Successfully added {0} to be minion of {1}.";

    }
}
