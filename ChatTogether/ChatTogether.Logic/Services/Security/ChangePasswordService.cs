using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using ChatTogether.Logic.Interfaces.Security;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services.Security
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly IChangePasswordTokenRepository changePasswordTokenRepository;
        private readonly IRandomStringGenerator randomStringGenerator;

        public ChangePasswordService(IChangePasswordTokenRepository changePasswordTokenRepository, IRandomStringGenerator randomStringGenerator)
        {
            this.changePasswordTokenRepository = changePasswordTokenRepository;
            this.randomStringGenerator = randomStringGenerator;
        }

        public async Task<bool> CheckToken(int accountId, string token)
        {
            ChangePasswordTokenDbo changePasswordTokenDbo = await changePasswordTokenRepository.GetAsync(x => x.AccountId == accountId && x.Token == token);

            if(changePasswordTokenDbo == null)
            {
                return false;
            }

            return true;
        }

        public async Task<ChangePasswordTokenDbo> CreateToken(int accountId)
        {
            ChangePasswordTokenDbo changePasswordTokenDbo = await changePasswordTokenRepository.GetAsync(x => x.AccountId == accountId);

            if(changePasswordTokenDbo != null)
            {
                await DeleteToken(accountId);
            }

            string token = randomStringGenerator.Generate();

            changePasswordTokenDbo = new ChangePasswordTokenDbo()
            {
                AccountId = accountId,
                Token = token
            };

            changePasswordTokenDbo = await changePasswordTokenRepository.CreateAsync(changePasswordTokenDbo);

            return changePasswordTokenDbo;
        }

        public async Task DeleteToken(int accountId)
        {
            await changePasswordTokenRepository.DeleteAsync(x => x.AccountId == accountId);
        }

        public async Task<ChangePasswordTokenDbo> Get(string token)
        {
            return await changePasswordTokenRepository.GetAsync(x => x.Token == token);
        }
    }
}
