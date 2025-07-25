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
            var name = ProfileNameTextBox.Text.Trim();
            System.Diagnostics.Debug.WriteLine($"Saving profile with name: {name}");
            if (string.IsNullOrEmpty(name))
            {
                var messageBox = new CustomMessageBox("Please enter a valid profile name.");
                messageBox.ShowDialog();
                return;
            }

            _profileManager.SaveProfile(name);
            RefreshProfileList();
        }

        private void LoadProfile_Click(object sender, RoutedEventArgs e)
        {
            if (ProfileListBox.SelectedItem == null)
            {
                MessageBox.Show("Select a profile to load.");
                return;
            }

            if(MultipleClientCheckBox.IsChecked == false)
            {
                RiotClientService.KillLeagueClient();
            }


            string profile = ProfileListBox.SelectedItem.ToString();
            _profileManager.LoadProfile(profile);
        }

        public void DeleteProfile_Click(object sender, RoutedEventArgs e)
        {
            if (ProfileListBox.SelectedItem == null)
            {
                MessageBox.Show("Select a profile to delete.");
                return;
            }

            _profileManager.DeleteProfile(ProfileListBox.SelectedItem.ToString());
            RefreshProfileList();
        }

        private void RefreshProfileList()
        {
            ProfileListBox.Items.Clear();
            foreach (var profile in _profileManager.ListProfiles())
            {
                ProfileListBox.Items.Add(profile);
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
    }
}
