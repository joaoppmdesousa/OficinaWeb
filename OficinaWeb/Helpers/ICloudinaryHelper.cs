using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OficinaWeb.Helpers
{
    public interface ICloudinaryHelper
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}
