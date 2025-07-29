using SummonerSwap.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SummonerSwap.Helpers
{
    internal class ConfigManager
    {
        private static readonly string ConfigFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SummonerSwap"
        );
        private static readonly string ConfigPath = Path.Combine(ConfigFolder, "config.json");

        public static AppConfig _config;

        static ConfigManager()
        {
            _config = LoadConfig();
        }

        public static AppConfig LoadConfig()
        {
            {
                if (!Directory.Exists(ConfigFolder))
                    Directory.CreateDirectory(ConfigFolder);

                if (!File.Exists(ConfigPath))
                {
                    _config = new AppConfig();
                    SaveConfig(); // Save defaults on first run
                    return new AppConfig();
                }

                var json = File.ReadAllText(ConfigPath);
                try
                {
                    return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error deserializing config: {ex.Message}");
                    _config = new AppConfig(); // Reset to defaults on error
                    return new AppConfig();
                }
            }
        }

        public static void SaveConfig()
        {
            if (_config == null)
            {
                System.Diagnostics.Debug.WriteLine("ConfigManager.SaveConfig called but _config is null!");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Saving config... " + JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true }));

            var json = JsonSerializer.Serialize(_config, new JsonSerializerOptions { WriteIndented = true });

            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            File.WriteAllText(ConfigPath, json);
        }
    }
}