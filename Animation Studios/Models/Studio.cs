using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Animation_Studios.Models
{
    public class Studio
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Country { get; set; }
        public string Headquarters { get; set; }
        public DateTime Founded { get; set; }
        public int NumberOfEmployees { get; set; }

        [XmlArray("Shows")]
        [XmlArrayItem("Show")]
        public List<Show> Shows { get; set; } = new List<Show>();
    }
}
