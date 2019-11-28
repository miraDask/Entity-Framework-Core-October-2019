namespace PetStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Validations.DataValidations;

    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        public ICollection<Food> Foods { get; set; } = new HashSet<Food>();

        public ICollection<Toy> Toys { get; set; } = new HashSet<Toy>();

    }
}
