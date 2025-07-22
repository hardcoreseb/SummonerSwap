using System.Diagnostics;
using System.IO;
using System.Windows;

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

        public static void LaunchClient()
        {
            if (File.Exists(RiotClientExe))
            {
                Process.Start(RiotClientExe, "--launch-product=league_of_legends --launch-patchline=live --allow-multiple-clients");
            }
            else
            {
                MessageBox.Show("Riot Client not found at expected path.");
            }
        }
    }
}
