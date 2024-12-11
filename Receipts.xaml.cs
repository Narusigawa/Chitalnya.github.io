using System.IO;
using System;
using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ClassLibraryEME;
using Range = Microsoft.Office.Interop.Word.Range;

namespace LibraryAssignmentWPF.UserControls
{
    public partial class Receipts : UserControl
    {
        public Receipts()
        {
            InitializeComponent();
            btnReturn.ButtonContent = Resex.btnReturn;
            btnWord.ButtonContent = Resex.btnWord;
            btnReturn.ButtonClickEvent += BtnReturn_ButtonClickEvent;
            btnWord.ButtonClickEvent += BtnWord_ButtonClickEvent;
            DisplayReceipts();
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
                List<string> items = DataBase.GetReceipts();
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

        private void BtnReturn_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            ReturnToMenu();
        }

        private void ReturnToMenu()
        {
            System.Windows.Window mainWindow = System.Windows.Window.GetWindow(this);
            Grid ReceiptsGrid = (Grid)mainWindow.FindName("ReceiptsGrid");
            ReceiptsGrid.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
        }

        public void DisplayReceipts()
        {
            try
            {
                listBox.ItemsSource = null;
                listBox.ItemsSource = DataBase.GetReceipts();
            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
