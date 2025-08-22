using Microsoft.AspNetCore.Http;

namespace Identity.Application.Interfaces.Services;

public interface IFileStorageService
{
    Task<string> SaveUserImageAsync(IFormFile file, CancellationToken cancellationToken);
}
