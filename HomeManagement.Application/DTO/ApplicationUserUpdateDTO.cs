using System.ComponentModel.DataAnnotations;

namespace HomeManagement.Application.DTO
{
    public class ApplicationUserUpdateDTO
    {
        [Required]
        [RegularExpression("^#[0-9a-fA-F]{6}([0-9a-fA-F]{2})?$", ErrorMessage = "Kolor musi być zapisany w formacie HEX.")]
        public string? CalendarEventBackgroundColor { get; set; }
    }
}
