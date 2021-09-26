using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> IsNicknameAvailable(string nickname);
        Task<UserDbo> ChangeNickname(string email, string nickname);
        Task<UserDbo> GetUser(string nickname);
        Task<Page<UserDbo>> GetUsers(int page, int pageSize, string search, Role? role);
        Task<UserDbo> Update(string email, UserDbo updatedUserDbo);
    }
}
