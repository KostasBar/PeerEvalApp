using PeerEvalAppAPI.Core.Enums;
using System.Text.RegularExpressions;

namespace PeerEvalAppAPI.DTO
{
    public class UserReadOnlyDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public UserRole? UserRole { get; set; }
        public required Group Group { get; set; }
        public DateTime InsertedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
