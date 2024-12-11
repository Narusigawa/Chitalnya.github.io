using ClassLibraryEME;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LibraryAssignmentWPF.UserControls
{
    public partial class ShowClubCustomers : UserControl
    {
        public ShowClubCustomers()
        {
            InitializeComponent();
            btnReturn.ButtonContent = Resex.btnReturn;
            btnWord.ButtonContent = Resex.btnWord;
            btnReturn.ButtonClickEvent += BtnReturn_ButtonClickEvent;
            btnWord.ButtonClickEvent += BtnWord_ButtonClickEvent;
            DisplayCustomersIds();
        }
        public void DisplayCustomersIds()
        {
            listBox.ItemsSource = null;
            listBox.ItemsSource = DataBase.GetCustomers();
        }
        private void BtnReturn_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            System.Windows.Window mainWindow = System.Windows.Window.GetWindow(this);
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;

            Grid ShowClubCustomersGrid = (Grid)mainWindow.FindName("ShowClubCustomersGrid");
            ShowClubCustomersGrid.Visibility = Visibility.Collapsed;
        }

        private void BtnWord_ButtonClickEvent(Object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application receiptsWordApp = new Microsoft.Office.Interop.Word.Application();
                receiptsWordApp.set_Visible(true);
                Document doc = receiptsWordApp.get_Documents().Add();
                Microsoft.Office.Interop.Word.Range range = doc.get_Content();
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                range.InsertAfter(title.Text + "\n\n");
                List<string> items = DataBase.GetCustomers();
                foreach (string item in items)
                {
                    range.InsertAfter(item.ToString() + "\n\n");
                }
                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(receiptsWordApp);

            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
