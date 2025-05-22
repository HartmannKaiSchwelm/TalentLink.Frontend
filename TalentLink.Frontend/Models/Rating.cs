namespace TalentLink.Frontend.Models
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid FromUserId { get; set; }
        public User FromUser { get; set; }
        public User ToUserId { get; set; }
        
int _score;
        public int Score
        {
            get => _score;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException(nameof(Score), "Rating muss zwischen 1 und 5 liegen.");
                _score = value;
            }
        }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
