using ChatTogether.Commons.GenericRepository;

namespace ChatTogether.Dal.Dbos.Security
{
    public class ConfirmEmailTokenDbo : DboModel<int>
    {
        public int AccountId { get; set; }
        public AccountDbo Account { get; set; }
        public string Token { get; set; }
    }
}
