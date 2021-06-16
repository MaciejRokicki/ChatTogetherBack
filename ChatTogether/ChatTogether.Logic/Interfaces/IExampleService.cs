using ChatTogether.Dal.Dbos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatTogether.Logic.Interfaces
{
    public interface IExampleService
    {
        Task<ExampleDbo> GetAsync(int id);
        Task<IEnumerable<ExampleDbo>> GetManyAsync();
        Task<IEnumerable<ExampleDbo>> GetManyAsync(string txt);
        Task<ExampleDbo> CreateAsync(ExampleDbo exampleDbo);
        Task<ExampleDbo> UpdateAsync(int id, ExampleDbo exampleDbo);
        Task DeleteAsync(int id);
    }
}
