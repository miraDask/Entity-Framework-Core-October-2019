namespace _02._VillainNames
{
    using System;

    public static class Constants
    {

        public const string ClientDB = "MinionsDB";


        public const string QueryText = @" SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  
                                             FROM Villains AS v 
                                             JOIN MinionsVillains AS mv ON v.Id = mv.VillainId 
                                         GROUP BY v.Id, v.Name 
                                           HAVING COUNT(mv.VillainId) > 3 
                                         ORDER BY COUNT(mv.VillainId)";

        public const string InputInvitationText = "Insert your server name please and press Enter!";
    }
}
