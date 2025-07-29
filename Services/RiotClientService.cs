using SummonerSwap.Views;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Windows;
using SummonerSwap.Models;
using SummonerSwap.Helpers;

namespace SummonerSwap.Services
{
    public static class RiotClientService
    {

        public static string RiotClientExePath
        {
            get => ConfigManager._config.RiotClientPath;
            set
            {
                ConfigManager._config.RiotClientPath = value;
                ConfigManager.SaveConfig();
            }
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
