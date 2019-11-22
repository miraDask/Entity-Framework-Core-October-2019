using System.Xml.Serialization;

namespace ProductShop.Dtos.Import
{
    [XmlType("CategoryProduct")]
    public class CategoryProductImportDto
    {

        public int CategoryId { get; set; }

        public int ProductId { get; set; }

    }
}
