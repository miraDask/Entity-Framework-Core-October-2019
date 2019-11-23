using System.Collections.Generic;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    [XmlType("Users")]
    public class UsersWithCountExportDto
    {
        [XmlElement("count")]
        public int Count { get; set; }

        [XmlArray("users")]
        public List<UserWithSoldProductsDto> UserWithSoldProductsDto { get; set; }
    }
}
