using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeerEvalAppAPI.Data
{
    public class Evaluation : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        
        public int EvaluatorUserId { get; set; }
        public int EvaluateeUserId { get; set; }
        public int EvaluationCycleId { get; set; }
        public DateTime TimeStamp { get; set; }
        [ForeignKey("EvaluatorUserId")]
        public required User EvaluatorUser { get; set; }
        [ForeignKey("EvaluateeUserId")]
        public required User EvaluateeUser { get; set; }
        [ForeignKey("EvaluationCycleId")]
        public virtual required EvaluationCycle EvaluationCycle { get; set; }
        public virtual ICollection<EvaluationAnswer>? Answers { get; set; }


    }
}
