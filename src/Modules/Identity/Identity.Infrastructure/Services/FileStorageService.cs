using Identity.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace Identity.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    public async Task<string> SaveUserImageAsync(IFormFile file, CancellationToken cancellationToken)
    {
        // Obtener ruta base (ej: /app/bin/Debug/net9.0/)
        var rootPath = Directory.GetCurrentDirectory();

        // Ruta a wwwroot/uploads/users
        var uploadsPath = Path.Combine(rootPath, "wwwroot", "uploads", "users");
        Directory.CreateDirectory(uploadsPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        // Retornar solo la ruta relativa para guardarla en DB
        return $"/uploads/users/{fileName}";
    }
}
