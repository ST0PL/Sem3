using ILS_WPF.Models;
using ILS_WPF.Services.Interfaces;
using System.IO;
using System.Text.Json;

namespace ILS_WPF.Services
{
    class ConfigurationService : IConfigurationService<Configuration?>
    {
        public Configuration? Configuration { get; private set; }
        private readonly string _filePath;

        public ConfigurationService(string filePath)
            => _filePath = filePath;

        public async Task LoadAsync()
        {
            if (!File.Exists(_filePath))
                await SaveAsync();
            using FileStream fs = File.OpenRead(_filePath);
            Configuration = await JsonSerializer.DeserializeAsync<Configuration>(fs);
        }

        public async Task SaveAsync()
        {
            Configuration ??= new();
            using FileStream fs = File.Create(_filePath);
            await JsonSerializer.SerializeAsync(fs, Configuration);
        }
    }
}
