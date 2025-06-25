using System.ComponentModel.DataAnnotations;

namespace TalentLink.Frontend.Models
{
    public class GuideCardCreateDto
    {
        [Required(ErrorMessage = "Typ ist erforderlich")]
        [StringLength(100, ErrorMessage = "Typ darf maximal 100 Zeichen lang sein")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Inhalt ist erforderlich")]
        [StringLength(1000, ErrorMessage = "Inhalt darf maximal 1000 Zeichen lang sein")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Reihenfolge ist erforderlich")]
        [Range(1, int.MaxValue, ErrorMessage = "Reihenfolge muss mindestens 1 sein")]
        public int OrderPosition { get; set; }
    }
}