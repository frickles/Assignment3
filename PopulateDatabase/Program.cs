using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PopulateDatabase
{
    public class Program
    {
        public static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            using var connection = new SqlConnection(@"Server=(local)\SQLExpress;Database=DataAccessGUIAssignment;Integrated Security=SSPI;");
            connection.Open();

            // Clear the database.
            new SqlCommand("DELETE FROM Tickets", connection).ExecuteNonQuery();
            new SqlCommand("DELETE FROM Screenings", connection).ExecuteNonQuery();
            new SqlCommand("DELETE FROM Movies", connection).ExecuteNonQuery();
            new SqlCommand("DELETE FROM Cinemas", connection).ExecuteNonQuery();

            // Load movies.
            string[] movieLines = File.ReadAllLines("SampleMovies.csv");
            foreach (string line in movieLines)
            {
                string[] parts = line.Split(',');
                string title = parts[0];
                string releaseDateString = parts[1];
                string runtimeString = parts[2];
                string posterPath = parts[3];

                int releaseYear = int.Parse(releaseDateString.Split('-')[0]);
                int releaseMonth = int.Parse(releaseDateString.Split('-')[1]);
                int releaseDay = int.Parse(releaseDateString.Split('-')[2]);
                var releaseDate = new DateTime(releaseYear, releaseMonth, releaseDay);

                int runtime = int.Parse(runtimeString);

                string sql = @"
                    INSERT INTO Movies (Title, ReleaseDate, Runtime, PosterPath)
                    VALUES (@Title, @ReleaseDate, @Runtime, @PosterPath)";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@ReleaseDate", releaseDate);
                command.Parameters.AddWithValue("@Runtime", runtime);
                command.Parameters.AddWithValue("@PosterPath", posterPath);
                command.ExecuteNonQuery();
            }

            // Load cinemas.
            string[] cinemaLines = File.ReadAllLines("SampleCinemas.csv");
            foreach (string line in cinemaLines)
            {
                string[] parts = line.Split(',');
                string city = parts[0];
                string name= parts[1];

                string sql = @"
                    INSERT INTO Cinemas (City, Name)
                    VALUES (@City, @Name)";
                using var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@City", city);
                command.Parameters.AddWithValue("@Name", name);
                command.ExecuteNonQuery();
            }

            // Generate random screenings.
            
            // Get all cinema IDs.
            var cinemaIDs = new List<int>();
            {
                string cinemaSql = "SELECT ID FROM Cinemas";
                using var cinemaCommand = new SqlCommand(cinemaSql, connection);
                using var cinemaReader = cinemaCommand.ExecuteReader();
                while (cinemaReader.Read())
                {
                    int id = Convert.ToInt32(cinemaReader["ID"]);
                    cinemaIDs.Add(id);
                }
            }

            // Get all movie IDs.
            var movieIDs = new List<int>();
            {
                string movieSql = "SELECT ID FROM Movies";
                using var movieCommand = new SqlCommand(movieSql, connection);
                using var movieReader = movieCommand.ExecuteReader();
                while (movieReader.Read())
                {
                    int id = Convert.ToInt32(movieReader["ID"]);
                    movieIDs.Add(id);
                }
            }

            // Create random screenings for each cinema.
            var random = new Random();
            foreach (int cinemaID in cinemaIDs)
            {
                // Choose a random number of screenings.
                int numberOfScreenings = random.Next(2, 6);
                foreach (int n in Enumerable.Range(0, numberOfScreenings)) {
                    // Pick a random movie.
                    int movieID = movieIDs[random.Next(movieIDs.Count)];

                    // Pick a random hour and minute.
                    int hour = random.Next(24);
                    double[] minuteOptions = { 0, 10, 15, 20, 30, 40, 45, 50 };
                    double minute = minuteOptions[random.Next(minuteOptions.Length)];
                    var time = TimeSpan.FromHours(hour) + TimeSpan.FromMinutes(minute);

                    // Insert the screening into the Screenings table.
                    string screeningSql = @"
                        INSERT INTO Screenings (MovieID, CinemaID, Time)
                        VALUES (@MovieID, @CinemaID, @Time)";
                    using var screeningCommand = new SqlCommand(screeningSql, connection);
                    screeningCommand.Parameters.AddWithValue("@MovieID", movieID);
                    screeningCommand.Parameters.AddWithValue("@CinemaID", cinemaID);
                    screeningCommand.Parameters.AddWithValue("@Time", time);
                    screeningCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
