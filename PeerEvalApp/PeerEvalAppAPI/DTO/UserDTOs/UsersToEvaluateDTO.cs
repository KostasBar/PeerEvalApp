namespace PeerEvalAppAPI.DTO.UserDTOs
{
    public class UsersToEvaluateDTO
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public string Email { get; set; } = null!;
    }
}
