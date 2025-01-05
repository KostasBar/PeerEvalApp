﻿using PeerEvalAppAPI.Data;

namespace PeerEvalAppAPI.Repositories
{
    public interface IEvaluationRepository
    {
        Task<Evaluation> GetEvaluationByIdAsync(int id);
        Task<List<Evaluation>> GetEvaluationsByCycle(int cycleId);
    }
}
