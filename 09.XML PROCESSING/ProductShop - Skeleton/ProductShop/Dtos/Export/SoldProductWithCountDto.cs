using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{

    [XmlType("SoldProducts")]
    public class SoldProductWithCountDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("products")]
        public List<SoldProductexportDto> Products { get; set; }
    }
}

