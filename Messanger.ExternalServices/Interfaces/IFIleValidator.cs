using Microsoft.AspNetCore.Http;

namespace Messanger.ExternalServices.Interfaces
{
    public interface IFileValidator
    {
        bool IsValid(IFormFile file);
    }
}
