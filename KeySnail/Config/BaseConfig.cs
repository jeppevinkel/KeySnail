using System;
using System.IO;

namespace KeySnail.Config;

public abstract class BaseConfig<T> where T : BaseConfig<T>, new()
{
    private static readonly string FilePath = ConfigManager.GetLocalFilePath($"{typeof(T).Name}.json");

    private static T? _instance;

    public static T Instance
    {
        get
        {
            if (_instance is null)
            {
                Load();
            }

            return _instance ?? throw new InvalidOperationException();
        }
    }

    public static void Load()
    {
        var fileExists = File.Exists(FilePath);
        
        _instance = (fileExists
            ? System.Text.Json.JsonSerializer.Deserialize<T>(File.ReadAllText(FilePath))
            : new T()) ?? new T();

        if (!fileExists)
        {
            Save();
        }
    }

    public static void Save()
    {
        var json = System.Text.Json.JsonSerializer.Serialize(Instance);
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
        File.WriteAllText(FilePath, json);
    }
}