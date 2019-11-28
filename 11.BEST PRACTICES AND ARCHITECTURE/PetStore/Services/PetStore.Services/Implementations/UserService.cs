namespace PetStore.Services.Implementations
{
    using System.Linq;

    using Data;
    using Data.Models;

    public class UserService : IUserService
    {
        private readonly PetStoreDbContext db;

        public UserService(PetStoreDbContext data) => this.db = data; 

        public bool Exists(int userId) => db.Users.Any(u => u.Id == userId);

        public void RegisterUser(string name, string email)
        {
            var user = new User()
            {
                Name = name,
                Email = email
            };

            db.Users.Add(user);
            db.SaveChanges();
        }
    }
}
