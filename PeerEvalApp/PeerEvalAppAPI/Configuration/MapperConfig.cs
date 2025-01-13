using AutoMapper;
using PeerEvalAppAPI.Data;
using PeerEvalAppAPI.DTO;

namespace PeerEvalAppAPI.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig() 
        {
            CreateMap<SubmitEvaluationDTO, Evaluation>()
               .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ForMember(dest => dest.EvaluatorUserId, opt => opt.MapFrom(src => src.EvaluatorUserId))
               .ForMember(dest => dest.EvaluateeUserId, opt => opt.MapFrom(src => src.EvaluateeUserId))
               .ForMember(dest => dest.EvaluationCycleId, opt => opt.MapFrom(src => src.EvaluationCycleId))
               .ForMember(dest => dest.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp))
               .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers));

            CreateMap<EvaluationAnswerDTO, EvaluationAnswer>()
               .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
               .ForMember(dest => dest.AnswerValue, opt => opt.MapFrom(src => src.AnswerValue));
        }
    }
}
