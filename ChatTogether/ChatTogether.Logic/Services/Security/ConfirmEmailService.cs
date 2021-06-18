using ChatTogether.Commons.RandomStringGenerator;
using ChatTogether.Dal.Dbos.Security;
using ChatTogether.Dal.Interfaces.Security;
using ChatTogether.Logic.Interfaces.Security;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services.Security
{
    public class ConfirmEmailService : IConfirmEmailService
    {
        private readonly IConfirmEmailTokenRepository confirmEmailTokenRepository;
        private readonly IRandomStringGenerator randomStringGenerator;

        public ConfirmEmailService(IConfirmEmailTokenRepository confirmEmailTokenRepository, IRandomStringGenerator randomStringGenerator)
        {
            this.confirmEmailTokenRepository = confirmEmailTokenRepository;
            this.randomStringGenerator = randomStringGenerator;
        }

        public async Task<bool> CheckToken(int accountId, string token)
        {
            ConfirmEmailTokenDbo confirmEmailTokenDbo = await confirmEmailTokenRepository.GetAsync(x => x.AccountId == accountId && x.Token == token);

            if (confirmEmailTokenDbo == null)
            {
                return false;
            }

            return true;
        }

        public async Task<ConfirmEmailTokenDbo> CreateToken(int accountId)
        {
            ConfirmEmailTokenDbo confirmEmailTokenDbo = await confirmEmailTokenRepository.GetAsync(x => x.AccountId == accountId);

            if (confirmEmailTokenDbo != null)
            {
                await DeleteToken(confirmEmailTokenDbo.AccountId);
            }

            string token = randomStringGenerator.Generate();

            confirmEmailTokenDbo = new ConfirmEmailTokenDbo()
            {
                AccountId = accountId,
                Token = token
            };

            confirmEmailTokenDbo = await confirmEmailTokenRepository.CreateAsync(confirmEmailTokenDbo);

            return confirmEmailTokenDbo;
        }

        public async Task DeleteToken(int accountId)
        {
            await confirmEmailTokenRepository.DeleteAsync(x => x.AccountId == accountId);
        }
    }
}
