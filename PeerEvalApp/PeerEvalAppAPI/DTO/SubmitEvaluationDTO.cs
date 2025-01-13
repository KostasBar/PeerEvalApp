using System.ComponentModel.DataAnnotations;

namespace PeerEvalAppAPI.DTO
{
    public class SubmitEvaluationDTO
    {
        [Required(ErrorMessage ="The evaluator's Id is mandatory!")]
        public int EvaluatorUserId { get; set; }
        [Required(ErrorMessage = "The evaluatee's Id is mandatory!")]
        public int EvaluateeUserId { get; set; }
        [Required(ErrorMessage = "The evaluation cycle is mandatory!")]
        public int EvaluationCycleId { get; set; }
        [Required(ErrorMessage = "The date is mandatory!")]
        public DateTime TimeStamp { get; set; }
        public List<EvaluationAnswerDTO> Answers { get; set; } = new List<EvaluationAnswerDTO>();
    }
}
