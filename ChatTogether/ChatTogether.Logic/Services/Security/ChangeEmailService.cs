using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using ChatTogether.Logic.Interfaces.Security;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services.Security
{
    public class ChangeEmailService : IChangeEmailService
    {
        private readonly IChangeEmailTokenRepository changeEmailTokenRepository;
        private readonly IRandomStringGenerator randomStringGenerator;

        public ChangeEmailService(IChangeEmailTokenRepository changeEmailTokenRepository, IRandomStringGenerator randomStringGenerator)
        {
            this.changeEmailTokenRepository = changeEmailTokenRepository;
            this.randomStringGenerator = randomStringGenerator;
        }

        public async Task<bool> CheckToken(int accountId, string token)
        {
            ChangeEmailTokenDbo changeEmailTokenDbo = await changeEmailTokenRepository.GetAsync(x => x.AccountId == accountId && x.Token == token);

            if(changeEmailTokenDbo == null)
            {
                return false;
            }

            return true;
        }

        public async Task<ChangeEmailTokenDbo> CreateToken(int accountId)
        {
            ChangeEmailTokenDbo changeEmailTokenDbo = await changeEmailTokenRepository.GetAsync(x => x.AccountId == accountId);

            if(changeEmailTokenDbo != null)
            {
                await DeleteToken(changeEmailTokenDbo.AccountId);
            }

            string token = randomStringGenerator.Generate();

            changeEmailTokenDbo = new ChangeEmailTokenDbo()
            {
                AccountId = accountId,
                Token = token
            };

            changeEmailTokenDbo = await changeEmailTokenRepository.CreateAsync(changeEmailTokenDbo);

            return changeEmailTokenDbo;
        }

        public async Task DeleteToken(int accountId)
        {
            await changeEmailTokenRepository.DeleteAsync(x => x.AccountId == accountId);
        }

        public async Task<ChangeEmailTokenDbo> Get(string token)
        {
            return await changeEmailTokenRepository.GetAsync(x => x.Token == token);
        }
    }
}
