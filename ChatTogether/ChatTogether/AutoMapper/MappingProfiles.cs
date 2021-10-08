using AutoMapper;
using ChatTogether.Commons.Page;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Ports.Dtos.Security;
using ChatTogether.Ports.HubModels;
using ChatTogether.ViewModels;
using ChatTogether.ViewModels.Security;
using System;

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
                    dest => dest.Created,
                    opt => opt.MapFrom(src => DateTime.SpecifyKind(src.Created, DateTimeKind.Utc))
                ).ForMember(
                    dest => dest.BlockedTo,
                    opt => opt.MapFrom(src => src.BlockedTo.HasValue ? DateTime.SpecifyKind(src.BlockedTo.Value, DateTimeKind.Utc) : src.BlockedTo)
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
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => src.Id)
                ).ForMember(
                    dest => dest.Role,
                    opt => opt.MapFrom(src => src.Account.Role)
                ).ForMember(
                    dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.BirthDate.HasValue ? DateTime.SpecifyKind(src.BirthDate.Value, DateTimeKind.Utc) : src.BirthDate)
                ).ForMember(
                    dest => dest.IsBlocked,
                    opt => opt.MapFrom(src => src.Account.BlockedAccountId == null ? false : true)
                );
            CreateMap<Page<UserDbo>, Page<UserViewModel>>();
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
                ).ForMember(
                    dest => dest.SendTime,
                    opt => opt.MapFrom(src => DateTime.SpecifyKind(src.SendTime, DateTimeKind.Utc))
                ).ForMember(
                    dest => dest.ReceivedTime,
                    opt => opt.MapFrom(src => DateTime.SpecifyKind(src.ReceivedTime, DateTimeKind.Utc))
                );
            CreateMap<MessageDbo, MessageHubModel>()
                .ForMember(
                    dest => dest.Nickname,
                    opt => opt.MapFrom(src => src.User.Nickname)
                ).ForMember(
                    dest => dest.SendTime,
                    opt => opt.MapFrom(src => DateTime.SpecifyKind(src.SendTime, DateTimeKind.Utc))
                ).ForMember(
                    dest => dest.ReceivedTime,
                    opt => opt.MapFrom(src => DateTime.SpecifyKind(src.ReceivedTime, DateTimeKind.Utc))
                );
            CreateMap<MessageHubModel, MessageDbo>();
        }
    }
}
