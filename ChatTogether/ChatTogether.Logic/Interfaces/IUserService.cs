using ChatTogether.Dal.Dbos;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsNicknameAvailable(string nickname);
        Task<UserDbo> ChangeNickname(string email, string nickname);
        Task<UserDbo> GetUser(string nickname);
        Task<UserDbo> Update(string email, UserDbo updatedUserDbo);
    }
}
