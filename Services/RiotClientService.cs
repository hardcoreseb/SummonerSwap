using SummonerSwap.Views;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace SummonerSwap.Services
{
    public class AppConfig
    {
        public string RiotClientPath { get; set; } = @"C:\Riot Games\Riot Client\RiotClientServices.exe";
    }

    public static class RiotClientService
    {
        private static readonly string ConfigFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "SummonerSwap"
        );
        private static readonly string ConfigPath = Path.Combine(ConfigFolder, "config.json");

        private static AppConfig config;

        static RiotClientService()
        {
            config = LoadConfig();
        }
        public static string RiotClientExePath
        {
            get => config.RiotClientPath;
            set
            {
                config.RiotClientPath = value;
                SaveConfig();
            }
        }

        private static AppConfig LoadConfig()
        {
            {
                if (!Directory.Exists(ConfigFolder))
                    Directory.CreateDirectory(ConfigFolder);

                if (!File.Exists(ConfigPath))
                {
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
                    return new AppConfig();
                }
            }
        }

        private static void SaveConfig()
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });

            if (!Directory.Exists(ConfigFolder))
                Directory.CreateDirectory(ConfigFolder);

            File.WriteAllText(ConfigPath, json);
        }

        public static bool IsValidRiotClientPath()
        {
            return File.Exists(RiotClientExePath) &&
                RiotClientExePath.EndsWith("RiotClientServices.exe", StringComparison.OrdinalIgnoreCase);
        }

        public static void KillClient()
        {
            foreach (var proc in Process.GetProcessesByName("RiotClientServices"))
            {
                proc.Kill();
                proc.WaitForExit();
            }
        }

        public static void KillLeagueClient()
        {
            foreach (var proc in Process.GetProcessesByName("LeagueClient"))
            {
                proc.Kill();
                proc.WaitForExit();
            }
        }

        public static void LaunchLeagueClient()
        {
            if (File.Exists(RiotClientExePath))
            {
                Process.Start(RiotClientExePath, "--launch-product=league_of_legends --launch-patchline=live --allow-multiple-clients");
            }
            else
            {
                var messageBox = new CustomMessageBox("Riot Client not found at expected path.", "Riot Client Path Error");
                messageBox.ShowDialog();
            }
        }

        public static bool CheckLeagueClientRunning()
        {
            var processes = Process.GetProcessesByName("LeagueClient");
            if (processes.Length > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void LaunchRiotClient()
        {
            if (File.Exists(RiotClientExePath))
            {
                Process.Start(RiotClientExePath);
            }
            else
            {
                var messageBox = new CustomMessageBox("Riot Client not found at expected path.", "Riot Client Path Error");
                messageBox.ShowDialog();
            }
        }
    }
}
