using SummonerSwap.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _showHelpOnStartup;
        public bool ShowHelpOnStartup
        {
            get => _showHelpOnStartup;
            set
            {
                if (_showHelpOnStartup != value)
                {
                    _showHelpOnStartup = value;
                    ConfigManager._config.ShowHelpOnStartup = !value;
                    ConfigManager.SaveConfig();
                    OnPropertyChanged(nameof(ShowHelpOnStartup));
                }
            }
        }
        public HelpWindow()
        {
            InitializeComponent();
            DataContext = this;
            ShowHelpOnStartup = !ConfigManager._config.ShowHelpOnStartup;
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

        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
