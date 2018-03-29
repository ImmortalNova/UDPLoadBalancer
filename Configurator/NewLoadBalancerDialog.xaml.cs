using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Configurator
{
    /// <summary>
    /// Interaction logic for NewLoadBalancerDialog.xaml
    /// </summary>
    public partial class NewLoadBalancerDialog : Window
    {
        public String ListenAddress
        {
            get { return listenAddressTextBox.Text; }
        }

        public int ListenPort
        {
            get { return int.Parse(listenPortTextBox.Text); }
        }

        public NewLoadBalancerDialog()
        {
            InitializeComponent();
            
            selectionAlgorithm.ItemsSource = Enum.GetValues(typeof(UDPLoadBalancer.LoadBalancer.ServerSelectionAlgorithm)).Cast<UDPLoadBalancer.LoadBalancer.ServerSelectionAlgorithm>();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.IsNumeric();
        }

        private void NumberValidationTextBoxPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!text.IsNumeric())
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close(true);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close(false);
        }

        public void Close(bool? result)
        {
            DialogResult = result;
            Close();
        }
    }
}
