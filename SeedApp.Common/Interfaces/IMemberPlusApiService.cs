using System.Threading.Tasks;

namespace SeedApp.Common.Interfaces
{
    public interface IMemberPlusApiService
    {
        Task<T> PostRequest<T>(string api, object data);

        Task<T> GetRequest<T>(string api);

        Task<string> GetRequest(string api);

        Task<bool> PostRequest(object data, string api);

        Task<string> PostRequest(string api, object data);

        Task<string> PostRequest(string api);

        Task<string> DeleteRequest(string api);

        Task<T> PostFileRequest<T>(byte[] fileBytes, string filename, string api);
	}
}