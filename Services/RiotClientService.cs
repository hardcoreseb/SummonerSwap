using System.Diagnostics;
using System.IO;
using System.Windows;
using SummonerSwap.Views;

namespace SummonerSwap.Services
{
    public static class RiotClientService
    {
        private static readonly string RiotClientExe = @"C:\Riot Games\Riot Client\RiotClientServices.exe";

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
            if (File.Exists(RiotClientExe))
            {
                Process.Start(RiotClientExe, "--launch-product=league_of_legends --launch-patchline=live --allow-multiple-clients");
            }
            else
            {
                var messageBox = new CustomMessageBox("Riot Client not found at expected path.");
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
            if (File.Exists(RiotClientExe))
            {
                Process.Start(RiotClientExe);
            }
            else
            {
                var messageBox = new CustomMessageBox("Riot Client not found at expected path.");
                messageBox.ShowDialog();
            }
        }
    }
}
