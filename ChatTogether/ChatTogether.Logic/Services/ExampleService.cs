using ChatTogether.Dal.Dbos;
using ChatTogether.Dal.Interfaces;
using ChatTogether.Logic.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Services
{
    public class ExampleService : IExampleService
    {
        private readonly IExampleRepository exampleRepository;

        public ExampleService(IExampleRepository exampleRepository)
        {
            this.exampleRepository = exampleRepository;
        }

        public async Task<ExampleDbo> CreateAsync(ExampleDbo exampleDbo)
        {
            ExampleDbo result = await exampleRepository.CreateAsync(exampleDbo);

            return result;
        }

        public async Task DeleteAsync(int id)
        {
            await exampleRepository.DeleteAsync(x => x.Id == id);
        }

        public async Task<ExampleDbo> GetAsync(int id)
        {
            ExampleDbo result = await exampleRepository.GetAsync(x => x.Id == id);

            return result;
        }

        public async Task<IEnumerable<ExampleDbo>> GetManyAsync()
        {
            IEnumerable<ExampleDbo> result = await exampleRepository.GetManyAsync();

            return result;
        }

        public async Task<IEnumerable<ExampleDbo>> GetManyAsync(string txt)
        {
            IEnumerable<ExampleDbo> result = await exampleRepository.GetManyAsync(x => x.Txt == txt);

            return result;
        }

        public async Task<ExampleDbo> UpdateAsync(int id, ExampleDbo exampleDbo)
        {
            ExampleDbo result = await exampleRepository.UpdateAsync(x => x.Id == id, exampleDbo);

            return result;
        }
    }
}
