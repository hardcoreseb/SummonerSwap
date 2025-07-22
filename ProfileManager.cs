using System;
using System.Collections.Generic;
using System.IO;
using SummonerSwap.Helpers;
using SummonerSwap.Services;

namespace SummonerSwap.Models
{
    public class ProfileManager
    {
        private readonly string RiotDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Data");
        private readonly string ProfilesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Profiles");

        public ProfileManager()
        {
            Directory.CreateDirectory(ProfilesPath);
        }

        public void SaveProfile(string name)
        {
            string profilePath = Path.Combine(ProfilesPath, name);
            if (Directory.Exists(profilePath))
                Directory.Delete(profilePath, true);

            FileManager.CopyDirectory(RiotDataPath, profilePath);
        }

        public void LoadProfile(string name)
        {
            string profilePath = Path.Combine(ProfilesPath, name);
            if (!Directory.Exists(profilePath))
                return;

            RiotClientService.KillClient();
            Directory.Delete(RiotDataPath, true);
            FileManager.CopyDirectory(profilePath, RiotDataPath);
            RiotClientService.LaunchClient();
        }

        public IEnumerable<string> ListProfiles()
        {
            if (!Directory.Exists(ProfilesPath))
                yield break;

            foreach (var dir in Directory.GetDirectories(ProfilesPath))
            {
                yield return Path.GetFileName(dir);
            }
        }
    }
}
