using ChatTogether.Commons.GenericRepository;
using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;

namespace ChatTogether.Dal.Repositories
{
    public class ExampleRepository : Repository<ExampleDbo>, IExampleRepository
    {
        private readonly ChatTogetherDbContext chatTogetherDbContext;

        public ExampleRepository(ChatTogetherDbContext chatTogetherDbContext) : base(chatTogetherDbContext)
        {
            this.chatTogetherDbContext = chatTogetherDbContext;
        }
    }
}
