namespace PeerEvalAppAPI.DTO.UserDTOs
{
    public class PastEvaluationsOfUserDTO
    {
        public int CycleId { get; set; }
        public string Department { get; set; } = null!;
        public DateTime CycleStartDate { get; set; }
        public DateTime CycleEndDate { get; set; }
        public float AvgEvaluationResult { get; set; }
    }
}
