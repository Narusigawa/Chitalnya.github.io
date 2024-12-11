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
using Range = Microsoft.Office.Interop.Word.Range;

namespace LibraryAssignmentWPF.UserControls
{
    public partial class ManageException : UserControl
    {
        public ManageException()
        {
            InitializeComponent();
            btnReturn.ButtonContent = Resex.btnReturn;
            btnWord.ButtonContent = Resex.btnWord;
            btnReturn.ButtonClickEvent += BtnReturn_ButtonClickEvent;
            btnWord.ButtonClickEvent += BtnWord_ButtonClickEvent;
            DisplayExceptions();
        }

        private void BtnWord_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.set_Visible(true);
                Document doc = wordApp.get_Documents().Add();
                Range range = doc.get_Content();
                range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;

                range.InsertAfter(title.Text + "\n\n");
                List<string> items = DataBase.GetExceptions();
                foreach (string item in items)
                {
                    range.InsertAfter(item.ToString() + "\n\n");
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnReturn_ButtonClickEvent(Object sender, RoutedEventArgs e)
        {
            ReturnToMenu();
        }

        private void ReturnToMenu()
        {
            System.Windows.Window mainWindow = System.Windows.Window.GetWindow(this);
            Grid ManageExceptionsGrid = (Grid)mainWindow.FindName("ManageExceptionsGrid");
            ManageExceptionsGrid.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
        }

        public void DisplayExceptions()
        {
            try
            {
                listBox.ItemsSource = null;
                listBox.ItemsSource = DataBase.GetExceptions();
            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
