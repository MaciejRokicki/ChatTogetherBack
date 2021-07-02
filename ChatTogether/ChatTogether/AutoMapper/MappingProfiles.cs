using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.HubModels;
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
            CreateMap<UserDbo, UserHubModel>();

            CreateMap<RoomDbo, RoomViewModel>();
            CreateMap<RoomDbo, RoomHubModel>();

            CreateMap<MessageDbo, MessageViewModel>()
                .ForMember(
                    dest => dest.Nickname,
                    opt => opt.MapFrom(src => src.User.Nickname)
                );
            CreateMap<MessageDbo, MessageHubModel>()
                .ForMember(
                    dest => dest.Nickname,
                    opt => opt.MapFrom(src => src.User.Nickname)
                );
            CreateMap<MessageHubModel, MessageDbo>();
        }
    }
}
