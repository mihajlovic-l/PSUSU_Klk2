using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Animation_Studios.Models
{
    public class Studio
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        [Required]
        public string Country { get; set; }

        public string Headquarters { get; set; }

        public DateTime Founded { get; set; }

        public int NumberOfEmployees { get; set; }

        // Navigation property - a studio has many shows
        public virtual ICollection<Show> Shows { get; set; } = new List<Show>();
    }
}
