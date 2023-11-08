using AutoMapper;
using BlazorQuizWASM.Server.Models.Domain;
using BlazorQuizWASM.Shared.DTO;

namespace BlazorQuizWASM.Server.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Question, QuestionRequestDto>().ReverseMap();
        }
    }
}
