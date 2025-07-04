﻿namespace TalentLink.Frontend.Models
{
    public class CreateJobDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public JobCategory Category { get; set; } = null!;
        public decimal PricePerHour { get; set; }
        public bool IsBoosted { get; set; }
        public Guid CreatedById { get; set; } // hinzugefügt für Backend-Kompatibilität

        public string?  ZipCode { get; set; } = null!; // hinzugefügt für Backend-Kompatibilität
        public string? City { get; set; } = null!;

        public int MinimumAge { get; set; } 
    }
}
