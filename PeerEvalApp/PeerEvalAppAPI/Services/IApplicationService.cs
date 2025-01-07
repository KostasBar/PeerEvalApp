namespace PeerEvalAppAPI.Services
{
    public interface IApplicationService
    {
        UserService UserService { get; }
        EvaluationCycleService EvaluationCycleService { get; }
    }
}
