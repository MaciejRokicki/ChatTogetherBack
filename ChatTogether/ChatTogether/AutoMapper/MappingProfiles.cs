using AutoMapper;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Ports.Dtos.Security;
using ChatTogether.Ports.HubModels;
using ChatTogether.ViewModels;
using ChatTogether.ViewModels.Security;

namespace ChatTogether.AutoMapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<BlockedAccountDbo, BlockedAccountViewModel>()
                .ForMember(
                    dest => dest.Email, 
                    opt => opt.MapFrom(src => src.Account.Email)
                ).ForMember(
                    dest => dest.Nickname,
                    opt => opt.MapFrom(src => src.Account.User.Nickname)
                ).ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src => src.Account.User.FirstName)
                ).ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src => src.Account.User.LastName)
                );

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
