using System.ComponentModel.DataAnnotations;

namespace PeerEvalAppAPI.Data
{
    public class Question : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(500)]
        public string Text { get; set; } = null!;
        public virtual ICollection<EvaluationAnswer> Answers { get; set; } = new List<EvaluationAnswer>();
    }
}
