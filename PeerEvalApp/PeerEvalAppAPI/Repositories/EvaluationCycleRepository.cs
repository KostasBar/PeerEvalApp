using Microsoft.EntityFrameworkCore;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;
using PeerEvalAppAPI.Exceptions;

namespace PeerEvalAppAPI.Repositories
{
    public class EvaluationCycleRepository : BaseRepository<EvaluationCycle>, IEvaluationCycleRepository
    {
        public EvaluationCycleRepository(PeerEvalAppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> OpenCycleExists() => await _dbContext.EvaluationsCycles.AnyAsync(ec => ec.Status == 1);

        //public async Task<EvaluationCycle?> UpdateEvaluationCycleAsync(EvaluationCycle evaluationCycle)
        //{
        //    EvaluationCycle? newEvaluationCycle = null;
        //    try
        //    {
        //        newEvaluationCycle = await _dbContext.EvaluationsCycles.FirstOrDefaultAsync(ec => ec.Id == evaluationCycle.Id);
        //        if(newEvaluationCycle is null)
        //        {
        //            throw new EntityNotFoundException("EvaluationCycle", "Evaluation Cycle with Id "+ evaluationCycle!.Id + " not found!");
        //        }

        //        _dbContext.EvaluationsCycles.Attach(evaluationCycle);
        //        _dbContext.Entry(evaluationCycle).State = EntityState.Modified;
        //    }
        //    catch (EntityNotFoundException)
        //    {

        //        throw;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //    return evaluationCycle;
        //}

        public async Task<EvaluationCycle?> UpdateEvaluationCycleAsync(EvaluationCycle evaluationCycle)
        {
            EvaluationCycle? existingCycle = await _dbContext.EvaluationsCycles
                                                             .FirstOrDefaultAsync(ec => ec.Id == evaluationCycle.Id);
            if (existingCycle == null)
            {
                throw new EntityNotFoundException("EvaluationCycle", "Evaluation Cycle with Id " + evaluationCycle.Id + " not found!");
            }

            // Map the updated values to the retrieved entity
            _dbContext.Entry(existingCycle).CurrentValues.SetValues(evaluationCycle);

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new Exception("A concurrency error occurred.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the evaluation cycle.", ex);
            }

            return existingCycle;
        }

    }
}
