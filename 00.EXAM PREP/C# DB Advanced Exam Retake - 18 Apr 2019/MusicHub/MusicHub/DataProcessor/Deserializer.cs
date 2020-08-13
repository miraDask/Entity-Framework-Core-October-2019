namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var writersDtos = JsonConvert.DeserializeObject<ImportWriterDto[]>(jsonString);

            foreach (var dto in writersDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var writer = new Writer
                {
                    Name = dto.Name,
                    Pseudonym = dto.Pseudonym
                };

                context.Writers.Add(writer);
                sb.AppendLine(string.Format(SuccessfullyImportedWriter, writer.Name));

            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var produsersDtos = JsonConvert.DeserializeObject<ImportProduserDto[]>(jsonString);

            foreach (var dto in produsersDtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var albums = new List<ImportAlbumDto>();

                foreach (var albumDto in dto.Albums)
                {
                    if (!IsValid(albumDto))
                    {
                        continue;
                    }

                    albums.Add(albumDto);
                }

                if (albums.Count != dto.Albums.Count())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var producer = new Producer
                {
                    Name = dto.Name,
                    Pseudonym = dto.Pseudonym,
                    PhoneNumber = dto.PhoneNumber
                };

                context.Producers.Add(producer);

                if (dto.PhoneNumber != null)
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithPhone, dto.Name, dto.PhoneNumber, albums.Count));
                }
                else
                {
                    sb.AppendLine(string.Format(SuccessfullyImportedProducerWithNoPhone, dto.Name, albums.Count));
                }
                

                foreach (var albumDto in dto.Albums)
                {
                    var album = new Album
                    {
                        Name = albumDto.Name,
                        ReleaseDate = DateTime.ParseExact(albumDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Producer = producer,
                    };

                    context.Albums.Add(album);
                }

            }

            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlSerializer = new XmlSerializer(typeof(List<ImportSongDto>),
                             new XmlRootAttribute("Songs"));

            List<ImportSongDto> dtos;

            using (var reader = new StringReader(xmlString))
            {
                dtos = (List<ImportSongDto>)xmlSerializer.Deserialize(reader);

                foreach (var dto in dtos)
                {
                    if (!IsValid(dto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var genre = Enum.TryParse(dto.Genre, out Genre genreResult);
                    var album = context.Albums.Find(dto.AlbumId);
                    var writer = context.Writers.Find(dto.WriterId);

                    if (!genre || album == null || writer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var song = new Song
                    {
                        Name = dto.Name,
                        Duration = TimeSpan.ParseExact(dto.Duration, "c", CultureInfo.InvariantCulture),
                        CreatedOn = DateTime.ParseExact(dto.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Genre = genreResult,
                        AlbumId = dto.AlbumId,
                        WriterId = dto.WriterId,
                        Price = dto.Price
                    };

                    context.Songs.Add(song);
                    sb.AppendLine(string.Format(SuccessfullyImportedSong, dto.Name, dto.Genre, dto.Duration));
                }

            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlSerializer = new XmlSerializer(typeof(List<ImportPerformerDto>),
                             new XmlRootAttribute("Performers"));

            List<ImportPerformerDto> dtos;

            using (var reader = new StringReader(xmlString))
            {
                dtos = (List<ImportPerformerDto>)xmlSerializer.Deserialize(reader);

                foreach (var dto in dtos)
                {
                    if (!IsValid(dto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var validSongIds = new List<ImportSongIdDto>();

                    foreach (var songDto in dto.Songs)
                    {
                        var song = context.Songs.Any(x => x.Id == songDto.Id);

                        if (song)
                        {
                            validSongIds.Add(songDto);
                        }
                    }

                    if (validSongIds.Count != dto.Songs.Length)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var performer = new Performer
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Age = dto.Age,
                        NetWorth = dto.NetWorth
                    };

                    context.Performers.Add(performer);
                    context.SaveChanges();

                    foreach (var song in dto.Songs)
                    {
                        var songPerformer = new SongPerformer
                        {
                            SongId = song.Id,
                            PerformerId = performer.Id
                        };

                        context.SongsPerformers.Add(songPerformer);

                    }
                    
                    context.SaveChanges();
                    sb.AppendLine(string.Format(SuccessfullyImportedPerformer, dto.FirstName, dto.Songs.Length));
                }

            }

            

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var result = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, result, true);
            // true => for all anotations check
        }
    }
}