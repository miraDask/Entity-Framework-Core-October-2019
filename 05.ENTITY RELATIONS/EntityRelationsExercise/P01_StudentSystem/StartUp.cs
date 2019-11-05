namespace P01_StudentSystem
{
    using System;
    using Data;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        static void Main()
        {
            var db = new StudentSystemContext();

            using (db)
            {
                db.Database.Migrate();
            }
        }
    }
}
