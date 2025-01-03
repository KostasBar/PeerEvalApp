namespace PeerEvalAppAPI.Data
{
    public class Group :BaseEntity
    {
        int Id {  get; set; }
        public string GroupName { get; set; } = null!;

        public virtual required ICollection<User> Users { get; set; }
    }
}
