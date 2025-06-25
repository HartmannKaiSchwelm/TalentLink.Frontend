using System.ComponentModel.DataAnnotations;

namespace TalentLink.Frontend.Models
{
    public class FaqItemCreateDto
    {
        [Required(ErrorMessage = "Question is required")]
        [StringLength(500, ErrorMessage = "Question cannot exceed 500 characters")]
        public string Question { get; set; } = string.Empty;

        [Required(ErrorMessage = "Answer is required")]
        [StringLength(2000, ErrorMessage = "Answer cannot exceed 2000 characters")]
        public string Answer { get; set; } = string.Empty;

        [Required(ErrorMessage = "OrderPosition is required")]
        [Range(1, int.MaxValue, ErrorMessage = "OrderPosition must be at least 1")]
        public int OrderPosition { get; set; }
    }
}
