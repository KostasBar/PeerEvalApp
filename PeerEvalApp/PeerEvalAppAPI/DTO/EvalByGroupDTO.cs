namespace PeerEvalAppAPI.DTO
{
    public class EvalByGroupDTO
    {
        public int CycleId { get; set; }
        public string CycleTitle { get; set; } = null!;
        public string EvaluateeFirstName { get; set; } = null!;
        public string EvaluateeLastName { get; set; } = null!;
        public double Average {  get; set; }
    }
}
