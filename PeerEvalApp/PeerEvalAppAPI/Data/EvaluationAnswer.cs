using System.ComponentModel.DataAnnotations.Schema;

namespace PeerEvalAppAPI.Data
{
    public class EvaluationAnswer : BaseEntity
    {
        public int Id { get; set; }
        public int EvaluationId { get; set; }
        public int QuestionId { get; set; }
        public string? AnswerValue { get; set; }
        [ForeignKey("EvaluationId")]
        public required virtual Evaluation Evaluation { get; set; }
        [ForeignKey("QuestionId")]
        public required virtual Question Question { get; set; }
    }
}
