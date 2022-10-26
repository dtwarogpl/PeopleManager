namespace PeopleManager.Infrastructure.Services.TextHandling;

public class FileStreamHandler : IAsyncDataFileHandler
{
    public async Task FileWriteAsync(string path, string msg)
    {

        CreateDirectoryIfNotPresent(path);

        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true);
        await using var sw = new StreamWriter(stream);
        await sw.WriteLineAsync(msg);
    }

    private static void CreateDirectoryIfNotPresent(string path)
    {
        var dir = new FileInfo(path).Directory;
        if (dir is { Exists: false })
        {
            dir.Create();
        }
    }

    public async Task<string> LoadTextAsync(string path)
    {
        if (!File.Exists(path))
        {
            return string.Empty;
        }

        using var sr = File.OpenText(path);
        return await sr.ReadToEndAsync();
    }

    
}