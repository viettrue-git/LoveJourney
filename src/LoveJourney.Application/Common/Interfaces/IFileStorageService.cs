namespace LoveJourney.Application.Common.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(Stream fileStream, string fileName, string folder = "");
    Task DeleteAsync(string path);
    string GetFullUrl(string relativePath);
}
