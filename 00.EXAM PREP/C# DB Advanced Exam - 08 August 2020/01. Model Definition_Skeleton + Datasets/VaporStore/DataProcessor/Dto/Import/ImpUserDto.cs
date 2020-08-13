namespace VaporStore.DataProcessor.Dto.Import
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ImpUserDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Range(3,103)]
        public int Age { get; set; }

        public ImpCardDto[] Cards { get; set; }
    }
}
