using Microsoft.Win32;
using SummonerSwap.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SummonerSwap.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            RiotPathTextBox.Text = RiotClientService.RiotClientExePath;          
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
            this.Close();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select RiotClientServices.exe",
                Filter = "Riot Client Executable|RiotClientServices.exe",
                InitialDirectory = @"C:\Riot Games\Riot Client\",
                CheckFileExists = true,
                CheckPathExists = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                RiotPathTextBox.Text = openFileDialog.FileName;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string selectedPath = RiotPathTextBox.Text;

            if (string.IsNullOrWhiteSpace(selectedPath))
            {
                var whiteSpaceErrorMessageBox = new CustomMessageBox("No path selected. Please select a valid RiotClientServices.exe path.", "Invalid Riot Path");
                whiteSpaceErrorMessageBox.ShowDialog();
                return;
            }

            if (!System.IO.File.Exists(selectedPath))
            {
                var fileNotFoundMessageBox = new CustomMessageBox("The selected file does not exist. Please select a valid RiotClientServices.exe path.", "File Not Found");
                fileNotFoundMessageBox.ShowDialog();
                return;
            }

            string fileName = System.IO.Path.GetFileName(selectedPath);
            if (!string.Equals(fileName, "RiotClientServices.exe", StringComparison.OrdinalIgnoreCase))
            {
                var invalidFileMessageBox = new CustomMessageBox("The selected file is not RiotClientServices.exe. Please select the correct file.", "Invalid File");
                invalidFileMessageBox.ShowDialog();
                return;
            }

            RiotClientService.RiotClientExePath = RiotPathTextBox.Text;

            var successSavedPathMessageBox = new CustomMessageBox("RiotClientServices.exe path saved successfully.", "Success");
            successSavedPathMessageBox.ShowDialog();
        }
    }
}
