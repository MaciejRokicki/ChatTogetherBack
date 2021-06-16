using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.ViewModels;

namespace ChatTogether.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<ExampleViewModel, ExampleDbo>()
                .ReverseMap();
        }
    }
}
