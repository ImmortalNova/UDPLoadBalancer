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
        private Configuration _config = null;

        public Configuration Config
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

        public LoadBalancerSection LoadBalancerConfig
        {
            get
            {
                return Config.GetSection("loadBalancers") as LoadBalancerSection;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            PropertyChanged += MainWindow_PropertyChanged;
        }

        private void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(String.Equals(e.PropertyName, "Config"))
            {
                if (loadBalancersListBox.ItemsSource == null)
                {
                    loadBalancersListBox.ItemsSource = LoadBalancerConfig.LoadBalancers;
                }

                loadBalancersListBox.Items.Refresh();
            }
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
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Title = "Create a new Configuration File",
                DefaultExt = ".config",
                Filter = "Configuration Files|*.config|All Files|*.*"
            };

            bool? done = sfd.ShowDialog();

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = sfd.FileName
            };

            Config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            Config.Sections.Add("loadBalancers", new LoadBalancerSection());

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

                Config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);

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
            if (e.AddedItems.Count > 0)
            {
                loadBalancerView.DataContext = e.AddedItems[0];
            }
        }

        private void NewLoadBalancer_Click(object sender, RoutedEventArgs e)
        {
            LoadBalancerElement newElement = new LoadBalancerElement();
            LoadBalancerConfig.LoadBalancers.Add(newElement);
            loadBalancersListBox.Items.Refresh();
        }

        private void DeleteLoadBalancer_Click(object sender, RoutedEventArgs e)
        {
            var loadBalancer = loadBalancersListBox.SelectedItem as LoadBalancerElement;
            if (loadBalancer != null)
            {
                LoadBalancerConfig.LoadBalancers.Remove(loadBalancer);
                loadBalancersListBox.ItemsSource = LoadBalancerConfig.LoadBalancers;
                loadBalancersListBox.Items.Refresh();
            }
        }

        private void NewLoadBalancerNode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteLoadBalancerNode_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
