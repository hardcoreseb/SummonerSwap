using System.IO;
using System.Windows;
using SummonerSwap.Services;
using SummonerSwap.Models;
using System.Windows.Controls;
using SummonerSwap.Views;
using System.Windows.Input;

namespace SummonerSwap
{
    public partial class MainWindow : Window
    {
        private ProfileManager _profileManager;

        public MainWindow()
        {
            InitializeComponent();
            _profileManager = new ProfileManager();
            RefreshProfileList();
        }

        private void SaveProfile_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckRiotClientPath()) return;

            var name = ProfileNameTextBox.Text.Trim();
            
            if (string.IsNullOrEmpty(name))
            {
                var messageBox = new CustomMessageBox("Please enter a valid profile name.", "Missing/Wrong Profile Name");
                messageBox.ShowDialog();
                return;
            }

            _profileManager.SaveProfile(name);
            RefreshProfileList();
        }

        private void LoadProfile_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckRiotClientPath()) return;

            if (ProfileListBox.SelectedItem is not ProfileViewModel selectedProfile)
            {
                var messageBox = new CustomMessageBox("Please select a profile to load.", "Profile Selection");
                messageBox.ShowDialog();          
                return;
            }

            if(MultipleClientCheckBox.IsChecked == false)
            {
                RiotClientService.KillLeagueClient();
            }

            _profileManager.LoadProfile(selectedProfile.Name);
        }

        public void DeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (ProfileListBox.SelectedItem is not ProfileViewModel selectedProfile)
            {
                var messageBox = new CustomMessageBox("Please select a profile to delete.", "Profile Selection");
                messageBox.ShowDialog();
                return;
            }

            _profileManager.DeleteProfile(selectedProfile.Name);
            System.Diagnostics.Debug.WriteLine($"Deleted profile: {selectedProfile.Name}");
            RefreshProfileList();
        }

        private void RefreshProfileList()
        {
            ProfileListBox.Items.Clear();

            string imageFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Summoner_Spells_Current_HD");

            foreach (var (name, meta) in _profileManager.ListProfiles())
            {
                string imagePath = Path.Combine(imageFolder, meta.Image ?? "default.png");

                var profileViewModel = new ProfileViewModel
                {
                    Name = name,
                    ImagePath = File.Exists(imagePath) ? imagePath : Path.Combine(imageFolder, "default.png")
                };

                ProfileListBox.Items.Add(profileViewModel);
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void AddProfile_Click(object sender, RoutedEventArgs e)
        {
            RiotClientService.KillLeagueClient();
            RiotClientService.KillClient();
            _profileManager.PrepareForNewProfile();
            RiotClientService.LaunchRiotClient();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }

        private bool CheckRiotClientPath()
        {
            if (!RiotClientService.IsValidRiotClientPath())
            {
                var messageBox = new CustomMessageBox("The Riot Client executable path is invalid or not found.\nPlease set the correct path in Settings.", 
                    "Invalid Riot Client Path");
                messageBox.ShowDialog();
                return false;
            }
            return true;
        }
    }
}
