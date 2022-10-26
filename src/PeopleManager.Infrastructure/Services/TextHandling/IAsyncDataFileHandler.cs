namespace PeopleManager.Infrastructure.Services.TextHandling;

public interface IAsyncDataFileHandler
{
    Task FileWriteAsync(string path, string msg);
    Task<string> LoadTextAsync(string path);
}