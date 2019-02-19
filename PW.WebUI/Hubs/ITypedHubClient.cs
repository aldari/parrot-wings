using System.Threading.Tasks;

namespace PW.Hubs
{
    public interface ITypedHubClient
    {
        Task TransactionNotify(string payload);
    }
}