namespace PetStore.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Validations.DataValidations;
    using static Validations.DataValidations.User;

    public class User
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(EmailMaxLength)]
        public string Email { get; set; }

        public ICollection<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
