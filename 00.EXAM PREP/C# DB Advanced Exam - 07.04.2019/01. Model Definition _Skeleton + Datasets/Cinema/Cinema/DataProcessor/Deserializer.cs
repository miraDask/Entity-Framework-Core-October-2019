namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Cinema.Data.Models;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var moviesFromJson = JsonConvert.DeserializeObject<MovieDto[]>(jsonString);
            var moviesToImport = new List<Movie>();

            foreach (var dto in moviesFromJson)
            {
                var moviesTitles = moviesToImport
                                       .Select(m => m.Title)
                                       .ToList();

                if (moviesTitles.Contains(dto.Title))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (IsValid(dto))
                {
                    var movie = new Movie
                    {
                        Title = dto.Title,
                        Genre = dto.Genre,
                        Duration = dto.Duration,
                        Rating = dto.Rating,
                        Director = dto.Director
                    };

                    moviesToImport.Add(movie);
                    sb.AppendLine(String.Format(SuccessfulImportMovie, dto.Title, dto.Genre, dto.Rating.ToString("f2")));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.Movies.AddRange(moviesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var hallsDtos = JsonConvert.DeserializeObject<HallWithSeatsCountDto[]>(jsonString);

            foreach (var dto in hallsDtos)
            {
                if (IsValid(dto))
                {
                    var hall = new Hall
                    {
                        Name = dto.Name,
                        Is4Dx = dto.Is4Dx,
                        Is3D = dto.Is3D
                    };

                    context.Halls.Add(hall);
                    AddSeatsToContext(dto.Seats, hall, context);

                    var projectionType = GetProjectionType(dto);
                    sb.AppendLine(String.Format(SuccessfulImportHallSeat, dto.Name, projectionType, dto.Seats));
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                }
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static string GetProjectionType(HallWithSeatsCountDto dto)
        {
            var projectionType = String.Empty;

            if (dto.Is3D && dto.Is4Dx)
            {
                projectionType = "4Dx/3D";
            }
            else if (!dto.Is3D && !dto.Is4Dx)
            {
                projectionType = "Normal";
            }
            else
            {
                projectionType = dto.Is4Dx ? "4Dx" : "3D";
            }

            return projectionType;
        }

        private static void AddSeatsToContext(int seatsCount, Hall hall, CinemaContext context)
        {
            for (int i = 0; i < seatsCount; i++)
            {
                var seat = new Seat
                {
                    Hall = hall
                };

                context.Seats.Add(seat);
            }

            context.SaveChanges();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var result = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, result, true);    // true => for all anotations check
        }
    }
}