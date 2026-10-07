namespace Playr.Models
{
    public class Game
    {
        public int AppId { get; set; }

        public string Name { get; set; } = "";

        public DateTime? ReleaseDate { get; set; }

        public string EstimatedOwners { get; set; } = "";

        public int? PeakCcu { get; set; } // peak concurrent users

        public double? Price { get; set; }

        public string HeaderImage { get; set; } = "";

        public int? MetacriticScore { get; set; }

        public int Positive { get; set; }

        public int Negative { get; set; }

        public int? Recommendations { get; set; }

        public int? AveragePlaytime { get; set; }

        public string Developers { get; set; } = "";

        public string Publishers { get; set; } = "";

        public string Categories { get; set; } = "";

        public string Genres { get; set; } = "";

        public string Tags { get; set; } = "";
    }
}