namespace PetStore.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PetStore.Services;
    using PetStore.Services.Models.Pet;
    using PetStore.Web.Models.Pets;

    public class PetsController : Controller
    {
        private readonly IPetService pets;

        public PetsController(IPetService pets)
        {
            this.pets = pets;
        }

        public IActionResult All(int page = 1)
        {
            var petsAll = this.pets.All(page);
            var totalPets = this.pets.TotalPets();

            var model = new AllPetsViewModel
            {
                AllPets = petsAll,
                CurrentPage = page,
                TotalPets = totalPets
            };

            return View(model);
        }
    }
}
