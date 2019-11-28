namespace PetStore.Services.Implementations
{
    using System.Linq;
    using PetStore.Data;

    public class CategoryService : ICategoryService
    {
        private readonly PetStoreDbContext db;
        public CategoryService(PetStoreDbContext data)
        {
            this.db = data;
        }

        public bool Exists(int categoryId)
            => this.db.Categories.Any(c => c.Id == categoryId);
    }
}
