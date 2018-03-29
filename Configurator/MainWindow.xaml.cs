using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
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

        private bool _isFileOpen = false;
        public bool IsFileOpen
        {
            get
            {
                return _isFileOpen;
            }

            set
            {
                _isFileOpen = value;
                OnPropertyChanged("IsFileOpen");
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

            loadBalancerSelectionAlgorithmComboBox.ItemsSource = Enum.GetValues(typeof(UDPLoadBalancer.LoadBalancer.ServerSelectionAlgorithm)).Cast<UDPLoadBalancer.LoadBalancer.ServerSelectionAlgorithm>();

            PropertyChanged += MainWindow_PropertyChanged;
        }

        private void MainWindow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(String.Equals(e.PropertyName, "Config"))
            {
                if (Config != null)
                {
                    IsFileOpen = true;

                    if (loadBalancersListBox.ItemsSource == null)
                    {
                        loadBalancersListBox.ItemsSource = LoadBalancerConfig.LoadBalancers;
                    }

                    loadBalancersListBox.Items.Refresh();
                }
                else
                {
                    IsFileOpen = false;
                }
            }
        }

        private void OnPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
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
            }
        }

        private void FileSaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Config != null)
            {
                Config.Save();
            }
        }

        private void FileSaveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (Config != null)
            {
                SaveFileDialog sfd = new SaveFileDialog()
                {
                    Title = "Save Configuration File As...",
                    DefaultExt = ".config",
                    Filter = "Configuration Files|*.config|All Files|*.*"
                };

                if (sfd.ShowDialog() == true)
                {
                    Config.SaveAs(sfd.FileName);
                }
            }
        }

        private void FileExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void LoadBalancersListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                loadBalancerView.DataContext = e.AddedItems[0];
            }
        }

        private void NewLoadBalancer_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NewLoadBalancerDialog();

            if (dlg.ShowDialog() == true)
            {
                LoadBalancerElement newElement = new LoadBalancerElement();
                newElement.ListenAddress = dlg.ListenAddress;
                newElement.ListenPort = dlg.ListenPort;
                LoadBalancerConfig.LoadBalancers.Add(newElement);
                loadBalancersListBox.Items.Refresh();
            }
        }

        private void DeleteLoadBalancer_Click(object sender, RoutedEventArgs e)
        {
            if (loadBalancersListBox.SelectedItem is LoadBalancerElement loadBalancer)
            {
                LoadBalancerConfig.LoadBalancers.Remove(loadBalancer);
                loadBalancersListBox.Items.Refresh();
            }
        }

        private void NewLoadBalancerNode_Click(object sender, RoutedEventArgs e)
        {
            if (loadBalancerView.DataContext is LoadBalancerElement context)
            {
                var dlg = new NewLoadBalancerNodeDialog();

                if (dlg.ShowDialog() == true)
                {
                    var el = new LoadBalancerNodeElement
                    {
                        Priority = dlg.Priority,
                        Address = dlg.NodeAddress,
                        Port = dlg.NodePort
                    };

                    context.Nodes.Add(el);
                    loadBalancerNodesListView.Items.Refresh();
                }
            }
        }

        private void DeleteLoadBalancerNode_Click(object sender, RoutedEventArgs e)
        {
            if (loadBalancerView.DataContext is LoadBalancerElement context)
            {
                if (loadBalancerNodesListView.SelectedItem is LoadBalancerNodeElement node)
                {
                    context.Nodes.Remove(node);
                    loadBalancerNodesListView.Items.Refresh();
                }
            }
        }
    }
}
