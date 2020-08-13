namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models.Enums;
    using VaporStore.DataProcessor.Dto.Export;

    public static class Serializer
	{
		public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
		{
			var genres = context.
				Genres
				.ToArray()
				.Where(x => genreNames.Contains(x.Name))
				.Select(x => new
				{
					x.Id,
					Genre = x.Name,
					Games = x.Games
					.Where(g => g.Purchases.Any())
					.Select(g => new
						{
							g.Id,
							Title = g.Name,
							Developer = g.Developer.Name,
							Tags = string.Join(", ", g.GameTags.Select(t => t.Tag.Name)),
							Players = g.Purchases.Count
						})
						.OrderByDescending(g => g.Players)
						.ThenBy(g => g.Id)
						.ToArray(),
					TotalPlayers = x.Games.Sum(y => y.Purchases.Count)
				})
				.OrderByDescending(x => x.TotalPlayers)
				.ThenBy(x => x.Id)
				.ToArray();

			var outputJson = JsonConvert.SerializeObject(genres, Newtonsoft.Json.Formatting.Indented);

			return outputJson;
		}

		public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
		{
			var perchaseType = Enum.Parse<PurchaseType>(storeType);

			var data = context.Users
				.ToArray()
				.Where(u => u.Cards.Any(c => c.Purchases.Any()))
				.Select(u => new ExpUserDto
				{ 
					UserName = u.Username,
					Purchases = u.Cards.SelectMany(c => c.Purchases.Where(p => p.Type == perchaseType)
						.Select(p => new ExpPurchaseDto 
					    {
					    	Card = c.Number,
					    	Cvc = c.Cvc,
					    	Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
							Game = new ExpGameDto { 
								Titile = p.Game.Name,
								Genre = p.Game.Genre.Name,
								Price = p.Game.Price,
							},
					    })).OrderBy(p => p.Date).ToArray(),
					TotalSpent = u.Cards.Sum(c => c.Purchases.Where(p => p.Type == perchaseType).Sum(p => p.Game.Price))
				})
				.Where(u => u.Purchases.Length > 0)
				.OrderByDescending(u => u.TotalSpent)
				.ThenBy(u => u.UserName)
				.ToList(); 

			var xmlSerializer = new XmlSerializer(typeof(List<ExpUserDto>),
											new XmlRootAttribute("Users"));



			var sb = new StringBuilder();
			var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

			using (var writer = new StringWriter(sb))
			{
				xmlSerializer.Serialize(writer, data, namespaces);
			}

			return sb.ToString().TrimEnd();
		}
	}
}