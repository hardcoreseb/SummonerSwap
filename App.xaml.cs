using SummonerSwap.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SummonerSwap
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Force early config load
            string configPath = RiotClientService.RiotClientExePath;
            System.Diagnostics.Debug.WriteLine($"[App.xaml.cs] Loaded RiotClientServices.exe path: {configPath}");
        }
    }

}
