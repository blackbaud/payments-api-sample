using System.Text.Json;

namespace Blackbaud.PaymentsAPI.Sample.Backend.DataAccess;

/// <summary>
/// Local file data adapter used as a stand-in for a real database
/// Reads and writes data to locally stored JSON document files
/// *Note*: Not to be used for any production workflow
/// </summary>
public class LocalFileDataAdapter
{
    private readonly IHostEnvironment _env;
    private const string DATA_STORE_DIRECTORY = "datastore";

    public LocalFileDataAdapter(IHostEnvironment env)
    {
        _env = env;
        EnsureDirectoryExists();
    }

    public async Task<T?> ReadDataAsync<T>()
        where T : new()
    {
        var filePath = GetFilePath<T>();
        if (!File.Exists(filePath))
        {
            return new T();
        }

        var json = await File.ReadAllTextAsync(filePath);
        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

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
