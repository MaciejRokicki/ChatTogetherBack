using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> IsNicknameAvailableAsync(string nickname);
        Task<UserDbo> ChangeNicknameAsync(string email, string nickname);
        Task<UserDbo> GetUserAsync(string nickname);
        Task<Page<UserDbo>> GetUsersAsync(int page, int pageSize, string search, Role? role);
        Task<UserDbo> UpdateAsync(string email, UserDbo updatedUserDbo);
    }
}
