namespace VaporStore.DataProcessor
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
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		private const string ErrorMessage = "Invalid Data";

		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();
			var dtos = JsonConvert.DeserializeObject<ImpGameDto[]>(jsonString);

            foreach (var dto in dtos)
            {
                if (!IsValid(dto) || dto.Tags.Length == 0)
                {
					sb.AppendLine(ErrorMessage);
					continue;
                }

				var developer = context.Developers.Where(x => x.Name == dto.Developer).FirstOrDefault();
				var genre = context.Genres.Where(x => x.Name == dto.Genre).FirstOrDefault();

				var game = new Game
				{
					Name = dto.Name,
					ReleaseDate = DateTime.ParseExact(dto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
					Price = dto.Price,
					Developer = developer ?? new Developer { Name = dto.Developer },
					Genre = genre ?? new Genre { Name = dto.Genre }
				};

				context.Games.Add(game);
				context.SaveChanges();
				var tags = new List<Tag>();

                foreach (var name in dto.Tags)
                {
					var tag = context.Tags.Where(x => x.Name == name).FirstOrDefault();
					if(tag == null)
                    {
						var newTag = new Tag { Name = name };
						context.Tags.Add(newTag);
						context.GameTags.Add(new GameTag { Tag = newTag, Game = game });
						context.SaveChanges();
                    }
                    else
                    {
						context.GameTags.Add(new GameTag { Tag = tag, Game = game });
						context.SaveChanges();
					}
				}

				sb.AppendLine($"Added {dto.Name} ({dto.Genre}) with {dto.Tags.Length} tags");
            }

			return sb.ToString().TrimEnd();
		}

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();
			var dtos = JsonConvert.DeserializeObject<ImpUserDto[]>(jsonString);

            foreach (var dto in dtos)
            {
                if (!IsValid(dto) || dto.Cards.Length == 0 || !dto.Cards.All(IsValid))
                {
					sb.AppendLine(ErrorMessage);
					continue;
				}

                foreach (var cardDto in dto.Cards)
                {
					var validCardType = Enum.TryParse<CardType>(cardDto.Type, false, out var cardType);
                    if (!validCardType)
                    {
						sb.AppendLine(ErrorMessage);
						continue;
					}
				}

				var user = new User
				{
					Username = dto.Username,
					FullName = dto.FullName,
					Age = dto.Age,
					Email = dto.Email,
					Cards = dto.Cards.Select(x => new Card
					{
						Number = x.Number,
						Cvc = x.Cvc,
						Type = Enum.Parse<CardType>(x.Type)
					}).ToArray()
				};

				context.Add(user);
				context.SaveChanges();
				sb.AppendLine($"Imported {dto.Username} with {dto.Cards.Length} cards");
            }

			return sb.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			var sb = new StringBuilder();
			var xmlSerializer = new XmlSerializer(typeof(List<ImpPurchaseDto>),
							 new XmlRootAttribute("Purchases"));

			List<ImpPurchaseDto> dtos;

			using (var reader = new StringReader(xmlString))
			{
				dtos = (List<ImpPurchaseDto>)xmlSerializer.Deserialize(reader);

				foreach (var dto in dtos)
				{

					var card = context.Cards.Where(x => x.Number == dto.Card).FirstOrDefault();
					var game = context.Games.Where(x => x.Name == dto.Titlle).FirstOrDefault();
					var validType = Enum.TryParse<PurchaseType>(dto.Type, false, out var type);

					if (!IsValid(dto) || card == null || game == null || !validType)
					{
						sb.AppendLine(ErrorMessage);
						continue;
					}

					var purchase = new Purchase
					{
						ProductKey = dto.Key,
						Type = type,
						Date = DateTime.ParseExact(dto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
						Game = game,
						Card = card
					};

					context.Add(purchase);
					context.SaveChanges();
					sb.AppendLine($"Imported {dto.Titlle} for {card.User.Username}");
				}

			}

			return sb.ToString().TrimEnd();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}