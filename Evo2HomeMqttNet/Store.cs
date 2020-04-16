using System;
using System.IO;
using System.Text.Json;
using Evo2HomeMqttNet.Models;
using Microsoft.Extensions.Logging;

namespace Evo2HomeMqttNet
{
    public class Store
    {
        private readonly ILogger<Store> _logger;
        private readonly EvoHomeSettings _evoHomeSettings;
        private readonly object _lockable = new object();

        private string Directory => _evoHomeSettings.FileLocation;

        public Store(ILogger<Store> logger, EvoHomeSettings evoHomeSettings)
        {
            _logger = logger;
            _evoHomeSettings = evoHomeSettings;
        }

        public void AddOrUpdate<T>(T value) where T : CacheObject, new()
        {
            lock (_lockable)
            {
                try
                {
                    value.SavedOn = DateTime.Now;
                    var jsonValue = JsonSerializer.Serialize<T>(value);
                    var fileName = $"{typeof(T).FullName}.out";

                    var fullPath = Path.Combine(Directory, fileName);

                    if (File.Exists(fullPath))
                    {
                        _logger.LogInformation($"Removing previously stored value for type {typeof(T).FullName}");
                        File.Delete(fullPath);
                    }

                    using var file = new StreamWriter(fullPath, false);
                    file.Write(jsonValue);

                    _logger.LogInformation($"Stored value for {typeof(T).FullName}");
                }
                catch (Exception e)
                {
                    throw new StoreSaveException(e.Message, e);
                }
            }
        }

        public T Get<T>() where T : CacheObject, new()
        {
            lock (_lockable)
            {
                try
                {
                    var fileName = $"{typeof(T).FullName}.out";

                    var fullPath = Path.Combine(Directory, fileName);

                    if (!File.Exists(fullPath))
                    {
                        return default;
                    }

                    using var file = new StreamReader(fullPath);
                    var jsonString = file.ReadToEnd();

                    var result = JsonSerializer.Deserialize<T>(jsonString);
                    var value = result.Expiry == TimeSpan.MaxValue || result.SavedOn.Add(result.Expiry) > DateTime.Now
                        ? result
                        : default;

                    _logger.LogInformation(value == default(T)
                        ? $"Value not found or out of date for type {typeof(T).FullName}"
                        : $"Retrieved value for type {typeof(T).FullName}");

                    return value;
                }
                catch (Exception e)
                {
                    throw new StoreException(e.Message, e);
                }
            }
        }
    }
}