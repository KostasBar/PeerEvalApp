using PeerEvalAppAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PeerEvalAppAPI.DTO
{
    public class UserSignUpDTO
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email must be between 5 and 100 characters.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W)^.{8,}$",
            ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character, and be at least 8 characters long.")]
        public string Password { get; set; } = null!;

        [EnumDataType(typeof(UserRole), ErrorMessage = "Invalid user role.")]
        public UserRole UserRole { get; set; }
        [Required(ErrorMessage = "User must belong to a group!")]
        public int GroupId { get; set; }
    }
}
