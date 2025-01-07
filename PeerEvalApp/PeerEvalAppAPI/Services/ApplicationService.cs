using AutoMapper;
using PeerEvalAppAPI.Repositories;

namespace PeerEvalAppAPI.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public UserService UserService => new(_unitOfWork, _mapper);
        public EvaluationCycleService EvaluationCycleService => new(_unitOfWork, _mapper);
    }
}
