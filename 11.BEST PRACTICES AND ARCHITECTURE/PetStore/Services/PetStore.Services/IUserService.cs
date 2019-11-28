namespace PetStore.Services
{

    public interface IUserService
    {
        void RegisterUser(string name, string email);

        bool Exists(int userId);
    }
}
