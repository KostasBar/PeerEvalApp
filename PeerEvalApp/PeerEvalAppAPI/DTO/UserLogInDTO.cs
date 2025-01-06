using System.ComponentModel.DataAnnotations;

namespace PeerEvalAppAPI.DTO
{
    public class UserLogInDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 50 characters.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W)^.{8,}$",
         ErrorMessage = "Password must contain at least one uppercase, one lowercase, one digit, and one special character, and be at least 8 characters long.")]
        public string? Password { get; set; }

    }
}
