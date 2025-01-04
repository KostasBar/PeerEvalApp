using System.ComponentModel.DataAnnotations;

namespace PeerEvalAppAPI.Data
{
    public class Group :BaseEntity
    {
        [Key]
        public int Id {  get; set; }
        [Required, MaxLength(128)]
        public string GroupName { get; set; } = null!;

        public virtual required ICollection<User> Users { get; set; }
    }
}
