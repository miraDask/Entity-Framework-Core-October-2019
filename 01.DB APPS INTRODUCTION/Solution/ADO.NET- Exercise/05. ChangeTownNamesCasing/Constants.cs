namespace _05._ChangeTownNamesCasing
{
    using System;

    public static class Constants
    {

        public const string ClientDB = "MinionsDB";

        public const string InputInvitationText = "Insert your server name please and press Enter!";

        public const string InputCountryName = "Insert country name and press Enter.";

        public const string UpdateQuery = @"UPDATE Towns
                                                  SET Name = UPPER(Name)
                                                WHERE CountryCode = (SELECT c.Id FROM Countries AS c WHERE c.Name = '{0}')";

        public const string TownNamesQuety = @"SELECT t.Name 
                                                 FROM Towns as t
                                                 JOIN Countries AS c ON c.Id = t.CountryCode
                                                WHERE c.Name = '{0}'";

        public const string NonTownsAffected = "No town names were affected.";

        public const string AffectedRows = "{0} town names were affected.";
    }
}
