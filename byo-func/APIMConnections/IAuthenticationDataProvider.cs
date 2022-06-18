using System.Threading.Tasks;

namespace MSHA.ApiConnections
{
    public interface IAuthenticationDataProvider
    {
        Task<string> GetAccessTokenAsync();

        Task<string> GetAccessTokenAsync(string resourceEndPoint);
    }
}
