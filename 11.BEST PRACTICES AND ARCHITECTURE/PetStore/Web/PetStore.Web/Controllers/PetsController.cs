namespace PetStore.Web.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using PetStore.Services;
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

        public IActionResult Delete(int id)
        {
            var pet = this.pets.PetDetails(id);

            if (pet == null)
            {
                return NotFound();
            }

            return View(pet);
        }

        public IActionResult ConfirmDelete(int id)
        {
            var isDeleted = this.pets.DeletePet(id);

            if (!isDeleted)
            {
                return BadRequest(); 
            }

            return RedirectToAction(nameof(All));
        }
    }
}
