using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using UDPLoadBalancer.Configuration;

namespace Configurator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private LoadBalancerSection _config = null;

        public LoadBalancerSection Config
        {
            get
            {
                return _config;
            }

            set
            {
                _config = value;
                OnPropertyChanged("Config");
            }
        }

        private bool _hasConfigOpen = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool HasConfigOpen
        {
            get
            {
                return _hasConfigOpen;
            }

            set
            {
                _hasConfigOpen = value;
                OnPropertyChanged("HasConfigOpen");   
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            ConfigLoaded += MainWindow_ConfigLoaded;
        }

        private void MainWindow_ConfigLoaded(object sender, EventArgs e)
        {
            loadBalancersListBox.ItemsSource = Config.LoadBalancers;
        }

        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event EventHandler ConfigLoaded;
        private void OnConfigLoaded()
        {
            if(ConfigLoaded != null)
            {
                ConfigLoaded(this, new EventArgs());
            }
        }

        private void FileNewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Config = new LoadBalancerSection();

            OnConfigLoaded();
        }

        private void FileOpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog()
            {
                Title = "Open a Configuration File",
                DefaultExt = ".config",
                Filter = "Configuration Files|*.config|All Files|*.*"
            };

            bool? done = d.ShowDialog();
            if (done.HasValue && done.Value == true)
            {
                var configMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = d.FileName
                };

                Configuration loadedConfig = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
                Config = loadedConfig.GetSection("loadBalancers") as LoadBalancerSection;

                OnConfigLoaded();
            }
        }

        private void FileSaveMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FileSaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FileExitMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadBalancersListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            loadBalancerView.DataContext = e.AddedItems[0];
        }
    }
}
