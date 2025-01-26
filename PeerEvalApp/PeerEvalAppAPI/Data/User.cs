﻿using PeerEvalAppAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PeerEvalAppAPI.Data
{
    public class User : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(255)]
        public string FirstName { get; set; } = null!;
        [Required, MaxLength (255)]
        public string LastName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public UserRole Role { get; set; }
        public int GroupId { get; set; }
        [ForeignKey("GroupId")]
        public virtual Group? Group { get; set; } 
        public int? ManagerId { get; set; }
        [ForeignKey("ManagerId")]
        public virtual User Manager { get; set; } = null!;
        public virtual ICollection<User> Subordinates { get; set; } = new List<User>();
        public virtual ICollection<Evaluation> EvaluationsMade { get; set; } = new List<Evaluation>();
        public virtual ICollection<Evaluation> EvaluationsReceived { get; set; } = new List<Evaluation>();
    }
}
