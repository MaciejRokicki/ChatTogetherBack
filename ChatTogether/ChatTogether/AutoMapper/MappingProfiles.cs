using AutoMapper;
using ChatTogether.Commons.Page;
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
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => src.Account.User.Id)
                )
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
                ).ForMember(
                    dest => dest.CreatedByEmail,
                    opt => opt.MapFrom(src => src.CreatedBy.Email)
                ).ForMember(
                    dest => dest.CreatedByNickname,
                    opt => opt.MapFrom(src => src.CreatedBy.User.Nickname)
                );

            CreateMap<Page<BlockedAccountDbo>, Page<BlockedAccountViewModel>>();

            CreateMap<RegistrationModel, AccountDto>();
            CreateMap<LoginModel, AccountDto>();

            CreateMap<AccountDto, AccountDbo>()
                .ReverseMap();

            CreateMap<UserDbo, UserViewModel>()
                .ForMember(
                    dest => dest.Role,
                    opt => opt.MapFrom(src => src.Account.Role)
                ).ForMember(
                    dest => dest.IsBlocked,
                    opt => opt.MapFrom(src => src.Account.BlockedAccountId == null ? false : true)
                );
            CreateMap<UserViewModel, UserDbo>();
            CreateMap<UserDbo, UserHubModel>();

            CreateMap<RoomDbo, RoomViewModel>()
                .ReverseMap();
            CreateMap<RoomDbo, RoomHubModel>()
                .ReverseMap();

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
