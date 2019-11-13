namespace FastFood.Web.Controllers
{
    using System.Linq;
    
    using Data;
    using Models;
    using ViewModels.Positions;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;

    public class PositionsController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public PositionsController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreatePositionInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Error", "Home");
            }

            var position = this.mapper.Map<Position>(model);

            this.context.Positions.Add(position);

            this.context.SaveChanges();

            return this.RedirectToAction("All", "Positions");
        }

        public IActionResult All()
        {
            var positions = this.context.Positions
                .ProjectTo<PositionsAllViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return this.View(positions);
        }

        public IActionResult Delete(int id)
        {
            var position = this.context.Positions.FirstOrDefault(o => o.Id == id);
            this.context.Positions.Remove(position);
            this.context.SaveChanges();

            return this.RedirectToAction("All", "Positions");
        }
    }
}
