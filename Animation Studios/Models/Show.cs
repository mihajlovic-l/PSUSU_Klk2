using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        public DateTime Started { get; set; }

        public DateTime? Ended { get; set; }

        public string Director { get; set; }

        public int NumberOfEpisodes { get; set; }

        public Genre Genre { get; set; }

        public int Rating { get; set; }

        public ShowStatus Status { get; set; }

        public bool Movie { get; set; }

        // Foreign key to Studio
        public Guid StudioId { get; set; }

        [ForeignKey("StudioId")]
        public virtual Studio Studio { get; set; }

        [NotMapped]
        public string GenreDisplay => Genre.ToString();

        [NotMapped]
        public string RatingDisplay => Rating == 0 ? "No rating" : Rating.ToString();
    }
}
