namespace PetStore
{
    using Microsoft.EntityFrameworkCore;
    using PetStore.Data;
    using System;

    public class StartUp
    {
        public static void Main()
        {
            var db = new PetStoreDbContext();

            using (db)
            {
                db.Database.Migrate();
            }
        }
    }
}
