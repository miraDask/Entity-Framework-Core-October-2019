namespace BookShop.DataProcessor.ImportDto
{
    using Newtonsoft.Json;

    public class BookIdDto
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }
    }
}
