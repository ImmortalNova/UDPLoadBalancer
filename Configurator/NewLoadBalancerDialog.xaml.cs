using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
