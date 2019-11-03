namespace P03_SalesDatabase
{
    using Microsoft.EntityFrameworkCore;
    using P03_SalesDatabase.Data;
    
    public class StartUp
    {
        public static void Main()
        {
            var db = new SalesContext();

            db.Database.Migrate();


        }
    }
}
