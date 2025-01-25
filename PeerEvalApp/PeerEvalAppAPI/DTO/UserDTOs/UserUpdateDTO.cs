using PeerEvalAppAPI.Core.Enums;

namespace PeerEvalAppAPI.DTO.UserDTOs
{
    public class UserUpdateDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password  { get; set; } = null!;
        public int GroupId { get; set; }
        public UserRole Role { get; set; }
    }
}
