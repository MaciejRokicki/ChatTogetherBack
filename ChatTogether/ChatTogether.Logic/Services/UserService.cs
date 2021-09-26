using ChatTogether.Commons.Exceptions;
using ChatTogether.Commons.Page;
using ChatTogether.Commons.Role;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Logic.Interfaces.Services;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepsitory)
        {
            this.userRepository = userRepsitory;
        }

        public async Task<UserDbo> ChangeNickname(string email, string nickname)
        {
            bool isNicknameAvailable = await userRepository.IsNicknameAvailable(nickname);

            if(!isNicknameAvailable)
            {
                throw new NicknameExistsException();
            }

            UserDbo userDbo = await userRepository.GetWithAccountAsync(x => x.Account.Email == email);

            userDbo.Nickname = nickname;
            userDbo = await userRepository.UpdateAsync(userDbo);

            return userDbo;
        }

        public async Task<UserDbo> GetUser(string nickname)
        {
            UserDbo userDbo = await userRepository.GetWithAccountAsync(x => x.Nickname == nickname);

            return userDbo;
        }

        public async Task<Page<UserDbo>> GetUsers(int page, int pageSize, string search, Role? role)
        {
            Page<UserDbo> users = await userRepository.GetPageAsync(page, pageSize, search, role);

            return users;
        }

        public async Task<bool> IsNicknameAvailable(string nickname)
        {
            bool isNicknameAvailable = await userRepository.IsNicknameAvailable(nickname);

            return isNicknameAvailable;
        }

        public async Task<UserDbo> Update(string email, UserDbo updatedUserDbo)
        {
            UserDbo userDbo = await userRepository.GetWithAccountAsync(x => x.Account.Email == email);

            List<string> propBlackList = new List<string>()
            {
                "Id",
                "Nickname",
                "AccountId",
                "Account",
                "Role",
                "IsBlocked"
            };

            foreach(PropertyInfo prop in typeof(UserDbo).GetProperties())
            {
                if (propBlackList.Contains(prop.Name))
                    continue;

                object newValue = prop.GetValue(updatedUserDbo);

                if(prop.Name == "BirthDate")
                {
                    prop.SetValue(userDbo, newValue);
                    continue;
                }

                if(newValue != null)
                {
                    prop.SetValue(userDbo, newValue);
                }
            }

            userDbo = await userRepository.UpdateAsync(userDbo);

            return userDbo;
        }
    }
}
