﻿namespace VaporStore.DataProcessor.Dto.Import
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;


    public class ImpGameDto
    {
        [Required]
        public string Name { get; set; }

        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Developer { get; set; }

        [JsonProperty("Tags")]
        public string[] Tags { get; set; }
     }
}
