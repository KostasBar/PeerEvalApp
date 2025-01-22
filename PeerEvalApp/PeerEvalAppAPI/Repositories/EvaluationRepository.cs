using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using System.Text;

namespace PeerEvalAppAPI.Repositories
{
    public class EvaluationRepository : BaseRepository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Evaluation?> GetEvaluationByIdAsync(int id)
            => await _dbContext.Evaluations.FirstOrDefaultAsync(e => e.Id == id);

        public async Task<List<Evaluation>? > GetEvaluationsByCycle(int cycleId)
        {
            List<Evaluation>? evaluations = await _dbContext.Evaluations
                .Where(e => e.EvaluationCycleId == cycleId).ToListAsync();
            return evaluations;
        }

        public async Task<List<EvalByGroupDTO>> GetEvaluationByGroup(int groupId, int cycleId)
        {
            //List<User> users = await _dbContext.Users
            //                        .Where(u => u.GroupId == groupId)
            //                        .ToListAsync();
            //List<EvaluationCycle> cycles = await _dbContext.EvaluationsCycles
            //                                    .Where(ec => ec.Id == cycleId)
            //                                    .ToListAsync();
            //List<Evaluation> evaluations = await _dbContext.Evaluations
            //                                    .Where(
            //                                        e => 
            //                                            cycles.Any(c => c.Id == e.Id) && 
            //                                            users.Any(u => u.Id == e.EvaluateeUserId)
            //                                    )
            //                                    .ToListAsync();
            //List<EvaluationAnswer> answers = await _dbContext.EvaluationsAnswers
            //                                    .Where(a => evaluations.Any(e => e.Id == a.EvaluationId))
            //                                    .ToListAsync();

            //var r = evaluations.Join(answers, )

            //var result = (from e in evaluations
            //              join ea in answers on e.Id equals ea.EvaluationId
            //              join u in users on e.EvaluateeUserId equals u.Id
            //              join ec in cycles on e.EvaluationCycleId equals ec.Id
            //              group ea by new { ec.Id, ec.Title, u.FirstName, u.LastName } into g
            //              select new EvalByGroupDTO
            //              {
            //                  CycleId = g.Key.Id,
            //                  CycleTitle = g.Key.Title,
            //                  EvaluateeFirstName = g.Key.FirstName,
            //                  EvaluateeLastName = g.Key.LastName,
            //                  Average = g.Average(x => float.TryParse(x.AnswerValue, out var val) ? val : 0)
            //              }).ToList();
            var sqlQuery = @"
        SELECT 
            ec.Id AS CycleId,
            ec.Title AS CycleTitle,
            u.FirstName AS EvaluateeFirstName,
            u.LastName AS EvaluateeLastName,
            CAST(AVG(CAST(ea.AnswerValue AS FLOAT)) AS FLOAT) AS Average -- Explicitly cast AVG to FLOAT
        FROM 
            Evaluations e
        INNER JOIN 
            EvaluationAnswers ea ON e.Id = ea.EvaluationId
        INNER JOIN 
            EvaluationCycles ec ON e.EvaluationCycleId = ec.Id
        INNER JOIN 
            Users u ON e.EvaluateeUserId = u.Id
        WHERE 
            u.GroupId = @groupId 
            AND e.EvaluationCycleId = @cycleId
        GROUP BY 
            ec.Id, ec.Title, u.FirstName, u.LastName";

            // Execute the raw SQL query and map to EvalByGroupDTO
            var result = await _dbContext.Database.SqlQueryRaw<EvalByGroupDTO>(sqlQuery,
                            new SqlParameter("@groupId", groupId),
                            new SqlParameter("@cycleId", cycleId))
                            .ToListAsync();

            return result;
        }
    }
}
