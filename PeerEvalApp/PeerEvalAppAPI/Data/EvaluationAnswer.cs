﻿using System.ComponentModel.DataAnnotations.Schema;

namespace PeerEvalAppAPI.Data
{
    public class EvaluationAnswer
    {
        public int Id { get; set; }
        public int EvaluationId { get; set; }
        public int QuestionId { get; set; }
        public string AnswerValue { get; set; } = null!;
        [ForeignKey("EvaluationId")]
        public required virtual Evaluation Evaluation { get; set; }
        [ForeignKey("QuestionId")]
        public required virtual Question Question { get; set; }
    }
}