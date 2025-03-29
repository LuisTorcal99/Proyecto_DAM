using System.Net.Http;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;

namespace Proyecto_DAM.Interfaces
{
    public interface IHttpsJsonClientProvider<T> where T : class
    {
        Task<IEnumerable<T>> GetAsync(string api_url);
        Task<T?> GetByIdAsync(string path, string id);
        Task<T?> PostAsync(string path, T data);
        Task<T?> PutAsync(string path, T data);
        Task<T?> PatchAsync(string path, T data);
        Task<bool> DeleteAsync(string path, string id);
        Task Authenticate(string path, HttpClient httpClient, HttpResponseMessage request);
        Task<T?> LoginPostAsync(string path, LoginDTO data);
        Task<T?> RegisterPostAsync(string path, RegistroDTO data);
    }
}
