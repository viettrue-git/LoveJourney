using LoveJourney.Application.Common.Interfaces;

namespace LoveJourney.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;

    public LocalFileStorageService(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> SaveAsync(Stream fileStream, string fileName, string folder = "")
    {
        var relativePath = string.IsNullOrEmpty(folder)
            ? fileName
            : Path.Combine(folder, fileName);

        var fullPath = Path.Combine(_basePath, relativePath);
        var directory = Path.GetDirectoryName(fullPath)!;
        Directory.CreateDirectory(directory);

        using var output = new FileStream(fullPath, FileMode.Create);
        await fileStream.CopyToAsync(output);

        return relativePath.Replace("\\", "/");
    }

    public Task DeleteAsync(string path)
    {
        var fullPath = Path.Combine(_basePath, path);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public string GetFullUrl(string relativePath)
    {
        return $"/uploads/{relativePath}";
    }
}
