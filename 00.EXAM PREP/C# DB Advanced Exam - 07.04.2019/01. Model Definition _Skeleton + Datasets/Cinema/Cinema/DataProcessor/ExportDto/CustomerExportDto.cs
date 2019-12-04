namespace Cinema.DataProcessor.ExportDto
{
    using Newtonsoft.Json;

    public class CustomerExportDto
    {
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Balance")]
        public string Balance { get; set; }
    }
}
