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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace LibraryAssignmentWPF.UserControls
{
    public partial class ClearableTextBox : UserControl
    {
        public ClearableTextBox()
        {
            DataContext = this;
            InitializeComponent();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private string boundText;

        public string BoundText
        {
            get { return boundText; }
            set
            {
                boundText = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("BoundText"));
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Text = string.Empty;
            txtInput.Focus();
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtInput.Text != string.Empty)
            {
                tbText.Visibility = Visibility.Hidden;
            }
            else tbText.Visibility = Visibility.Visible;
        }
    }
}
