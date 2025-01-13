using System.ComponentModel.DataAnnotations;

namespace PeerEvalAppAPI.DTO.EvaluationCycleDTOs
{
    public class InitiateCycleDTO
    {
        [Required]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Evaluation Cycle Title must be between 5 and 30 characters long!")]
        public string? Title { get; set; } = null;
        public int Weeks { get; set; }

    }
}
