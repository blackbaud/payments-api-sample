using System.Text.Json;

namespace Blackbaud.PaymentsAPITutorial.DataAccess;

public class LocalFileDataAdapter
{
    private readonly IHostEnvironment _env;

    public LocalFileDataAdapter(IHostEnvironment env)
    {
        _env = env;
    }

    public async Task<T> ReadDataAsync<T>()
    {
        var filePath = GetFilePath<T>();
        if (!File.Exists(filePath))
        {
            return default;
        }

        var json = await File.ReadAllTextAsync(filePath);
        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task WriteDataAsync<T>(T data)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        var json = JsonSerializer.Serialize(data, options);
        await File.WriteAllTextAsync(GetFilePath<T>(), json);
    }

    private string GetFilePath<T>()
    {
        return Path.Combine(_env.ContentRootPath, "datastore", typeof(T).Name + ".json");
    }
}
