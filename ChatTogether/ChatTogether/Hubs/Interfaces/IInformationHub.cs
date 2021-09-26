using System.Threading.Tasks;

namespace ChatTogether.Hubs.Interfaces
{
    public interface IInformationHub
    {
        Task Signout(string information);
    }
}
