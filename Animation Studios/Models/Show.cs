using System;
using System.Xml.Serialization;

namespace Animation_Studios.Models
{
    public enum ShowStatus
    {
        PlanToWatch,
        OnHold,
        Watching,
        Completed,
        Dropped
    }

    public class Show
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime Started { get; set; }
        public DateTime Ended { get; set; }
        public string Director { get; set; }
        public int NumberOfEpisodes { get; set; }
        public Genre Genre { get; set; }
        public int Rating { get; set; }
        public ShowStatus Status { get; set; }
        public bool Movie { get; set; }

        [XmlIgnore]
        public string GenreDisplay => Genre.ToString();

        [XmlIgnore]
        public string RatingDisplay => Rating == 0 ? "No rating" : Rating.ToString();
    }
}
