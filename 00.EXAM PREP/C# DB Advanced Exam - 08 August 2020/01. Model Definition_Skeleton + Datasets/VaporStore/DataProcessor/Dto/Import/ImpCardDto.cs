namespace VaporStore.DataProcessor.Dto.Import
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;

    public class ImpCardDto
    {

        [Required]
        [RegularExpression(@"^(\d{4}) (\d{4}) (\d{4}) (\d{4})$")]
        public string Number { get; set; }

        [Required]
        [JsonProperty("CVC")]
        [RegularExpression(@"^(\d{3})$")]
        public string Cvc { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
