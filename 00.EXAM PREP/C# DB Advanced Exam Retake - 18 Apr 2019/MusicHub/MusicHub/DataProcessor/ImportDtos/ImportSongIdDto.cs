namespace MusicHub.DataProcessor.ImportDtos
{
    using System.Xml.Serialization;

    [XmlType("Song")]
    public class ImportSongIdDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
