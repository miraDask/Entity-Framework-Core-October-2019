namespace Cinema.DataProcessor.ExportDto
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class MovieWithCustomersExportDto
    {
        [JsonProperty("MovieName")]
        public string MovieName { get; set; }

        [JsonProperty("Rating")]
        public string Rating { get; set; }

        [JsonProperty("TotalIncomes")]
        public string TotalIncomes { get; set; }

        [JsonProperty("Customers")]
        public List<CustomerExportDto> Customers { get; set; } = new List<CustomerExportDto>();
    }
}
