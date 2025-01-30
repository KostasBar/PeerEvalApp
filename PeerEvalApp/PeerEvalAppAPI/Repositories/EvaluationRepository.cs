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
