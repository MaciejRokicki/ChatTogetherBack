using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Ports.Dtos.Security;
using ChatTogether.ViewModels;
using ChatTogether.ViewModels.Security;

namespace ChatTogether.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegistrationModel, AccountDto>();
            CreateMap<LoginModel, AccountDto>();

            CreateMap<AccountDto, AccountDbo>()
                .ReverseMap();

            CreateMap<UserDbo, UserViewModel>()
                .ReverseMap();
        }
    }
}
