namespace FastFood.Web.Controllers
{
    using System.Linq;
    
    using Data;
    using ViewModels.Categories;
    using FastFood.Models;
    
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;

    public class CategoriesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public CategoriesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var categorie = this.mapper.Map<Category>(model);
            this.context.Categories.Add(categorie);
            this.context.SaveChanges();

            return RedirectToAction("All", "Categories");
        }

        public IActionResult All()
        {
            var categories = this.context
                .Categories
                .ProjectTo<CategoryAllViewModel>(this.mapper.ConfigurationProvider)
                .ToList();

            return this.View(categories);
        }

        public IActionResult Delete(int id)
        {
            var category = this.context.Categories.FirstOrDefault(o => o.Id == id);
            this.context.Categories.Remove(category);
            this.context.SaveChanges();

            return this.RedirectToAction("All", "Categories");
        }
    }
}
