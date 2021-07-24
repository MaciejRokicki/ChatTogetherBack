using ChatTogether.Commons.Exceptions;
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
        private readonly IUserRepository userRepsitory;

        public UserService(IUserRepository userRepsitory)
        {
            this.userRepsitory = userRepsitory;
        }

        public async Task<UserDbo> ChangeNickname(string email, string nickname)
        {
            bool isNicknameAvailable = await userRepsitory.IsNicknameAvailable(nickname);

            if(!isNicknameAvailable)
            {
                throw new NicknameExistsException();
            }

            UserDbo userDbo = await userRepsitory.GetWithAccountAsync(x => x.Account.Email == email);

            userDbo.Nickname = nickname;
            userDbo = await userRepsitory.UpdateAsync(userDbo);

            return userDbo;
        }

        public async Task<UserDbo> GetUser(string nickname)
        {
            UserDbo userDbo = await userRepsitory.GetAsync(x => x.Nickname == nickname);

            return userDbo;
        }

        public async Task<bool> IsNicknameAvailable(string nickname)
        {
            bool isNicknameAvailable = await userRepsitory.IsNicknameAvailable(nickname);

            return isNicknameAvailable;
        }

        public async Task<UserDbo> Update(string email, UserDbo updatedUserDbo)
        {
            UserDbo userDbo = await userRepsitory.GetWithAccountAsync(x => x.Account.Email == email);

            List<string> propBlackList = new List<string>()
            {
                "Id",
                "Nickname",
                "AccountId",
                "Account"
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

            userDbo = await userRepsitory.UpdateAsync(userDbo);

            return userDbo;
        }
    }
}
