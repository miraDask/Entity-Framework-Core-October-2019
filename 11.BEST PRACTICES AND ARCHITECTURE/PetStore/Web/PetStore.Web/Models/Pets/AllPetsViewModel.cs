namespace PetStore.Web.Models.Pets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Services.Models.Pet;
    
    public class AllPetsViewModel
    {
        public IEnumerable<PetListingServiceModel> AllPets { get; set; }

        public int TotalPets { get; set; }

        public int CurrentPage { get; set; }

        public int PrevPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        public bool PreviousDisabled => this.CurrentPage == 1;

        public bool NextDisabled => this.CurrentPage == Math.Ceiling((double)this.TotalPets / 25);

    }
}
