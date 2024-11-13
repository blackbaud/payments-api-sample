using System.Text.Json;

namespace Blackbaud.PaymentsAPITutorial.DataAccess;

public class LocalFileDataAdapter
{
    private readonly IHostEnvironment _env;
    private const string DATA_STORE_DIRECTORY = "datastore";

    public LocalFileDataAdapter(IHostEnvironment env)
    {
        _env = env;
        EnsureDirectoryExists();
    }

    public async Task<T> ReadDataAsync<T>()
        where T : new()
    {
        var filePath = GetFilePath<T>();
        if (!File.Exists(filePath))
        {
            return new T();
        }

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task WriteDataAsync<T>(T data)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };

        var json = JsonSerializer.Serialize(data, options);
        await File.WriteAllTextAsync(GetFilePath<T>(), json);
    }

    private string GetFilePath<T>()
    {
        return Path.Combine(_env.ContentRootPath, DATA_STORE_DIRECTORY, typeof(T).Name + ".json");
    }

    private void EnsureDirectoryExists()
    {
        var directory = Path.Combine(_env.ContentRootPath, DATA_STORE_DIRECTORY);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }
}
