using AutoMapper;
using ChatTogether.Commons.Pagination.Models;
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

            CreateMap<PaginationPage<RoomDbo>, PaginationPage<RoomViewModel>>();
            CreateMap<PaginationPage<RoomDbo>, PaginationPage<RoomHubModel>>();
        }
    }
}
