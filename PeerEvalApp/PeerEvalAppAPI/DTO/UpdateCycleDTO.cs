using System.ComponentModel.DataAnnotations;

namespace PeerEvalAppAPI.DTO
{
    public class UpdateCycleDTO
    {
        [Required (ErrorMessage ="Cannot update Evaluation Cycle without an ID!")]
        public int Id { get; set; }
        public int Status {  get; set; }
        public int EndWeek { get; set; }
    }
}
