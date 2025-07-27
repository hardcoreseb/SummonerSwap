using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using SummonerSwap.Helpers;
using SummonerSwap.Services;
using SummonerSwap.Views;

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
            if (!RiotClientService.CheckLeagueClientRunning())
            {
                var messageBox = new CustomMessageBox("Please be logged into League of Legends to save an account.", "Save Profile Error");
                messageBox.ShowDialog();
                return;
            }

            string profilePath = Path.Combine(ProfilesPath, name);
            if (Directory.Exists(profilePath))
                Directory.Delete(profilePath, true);

            FileManager.CopyDirectory(RiotDataPath, profilePath);
            System.Diagnostics.Debug.WriteLine($"Profile saved to {profilePath}");

            var imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Summoner_Spells_Current_HD");
            System.Diagnostics.Debug.WriteLine($"Looking for images in: {imageFolder}");
            var images = Directory.GetFiles(imageFolder, "*.png");
            if (images.Length == 0)
            {
                MessageBox.Show("No .png images found in: " + imageFolder);
                return;
            }
            var random = new Random();

            var selectedImage = Path.GetFileName(images[random.Next(images.Length)]);

            var metaData = new ProfileMetaData
            {
                Image = selectedImage,
                Created = DateTime.Now
            };

            var metaDataPath = Path.Combine(profilePath, "metadata.json");
            File.WriteAllText(metaDataPath, System.Text.Json.JsonSerializer.Serialize(metaData));
        }

        public void LoadProfile(string name)
        {
            string profilePath = Path.Combine(ProfilesPath, name);
            if (!Directory.Exists(profilePath))
                return;

            RiotClientService.KillClient();
            Directory.Delete(RiotDataPath, true);
            FileManager.CopyDirectory(profilePath, RiotDataPath);
            RiotClientService.LaunchLeagueClient();
        }

        public void DeleteProfile(string name)
        {
            string profilePath = Path.Combine(ProfilesPath, name);
            if (Directory.Exists(profilePath))
            {
                Directory.Delete(profilePath, true);
                var messageBox = new CustomMessageBox($"Profile '{name}' deleted successfully.", "Deletion success!");
                messageBox.ShowDialog();                
            }
        }

        public void PrepareForNewProfile()
        {
            if (Directory.Exists(RiotDataPath))
            {
                Directory.Delete(RiotDataPath, true);
            }
        }

        public IEnumerable<(string Name, ProfileMetaData MetaData)> ListProfiles()
        {
            if (!Directory.Exists(ProfilesPath))
                yield break;

            foreach (var dir in Directory.GetDirectories(ProfilesPath))
            {
                string name = Path.GetFileName(dir);
                string metaDataPath = Path.Combine(dir, "metadata.json");

                ProfileMetaData metaData = new ProfileMetaData();

                if (File.Exists(metaDataPath))
                {
                    try
                    {
                        var metaDataJson = File.ReadAllText(metaDataPath);
                        metaData = System.Text.Json.JsonSerializer.Deserialize<ProfileMetaData>(metaDataJson);
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to read metadata for profile: {name}");
                        // If metadata fails, we can still use the default image
                    }
                }

                yield return (name, metaData);
            }
        }
    }
}
