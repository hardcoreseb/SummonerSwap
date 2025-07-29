using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonerSwap.Models
{
    public class AppConfig
    {
        public string RiotClientPath { get; set; } = @"C:\Riot Games\Riot Client\RiotClientServices.exe";
        public bool ShowHelpOnStartup { get; set; } = true;
    }
}
