using ClassLibraryEME;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
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
using Range = Microsoft.Office.Interop.Word.Range;

namespace LibraryAssignmentWPF.UserControls
{
    public partial class ShowComics : UserControl
    {
        public ShowComics()
        {
            InitializeComponent();
            btnReturn.ButtonContent = Resex.btnReturn;
            btnWord.ButtonContent = Resex.btnWord;
            btnFind.ButtonContent = Resex.btnFind;
            btnRevert.ButtonContent = Resex.btnShowComics;
            btnReturn.ButtonClickEvent += BtnReturn_ButtonClickEvent;
            btnWord.ButtonClickEvent += BtnWord_ButtonClickEvent;
            btnFind.ButtonClickEvent += BtnFind_ButtonClickEvent;
            btnRevert.ButtonClickEvent += BtnRevert_ButtonClickEvent;
            DisplayComics();
        }

        private void BtnFind_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            if (textBox.txtInput.Text == string.Empty)
            {
                MessageBox.Show("Пожалуйста, введите имя в поле ввода, чтобы отсортировать список!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                try
                {
                    var comicList = DataBase.FilterItemsByName(textBox.txtInput.Text);
                    var itemsList = comicList;
                    for (int i = 0; i < itemsList.Count; i++)
                    {
                        if (itemsList[i].GetType() == typeof(Journal)) comicList.Remove(itemsList[i]);
                    }
                    if (comicList.Count == 0)
                    {
                        MessageBox.Show("Введеное вами имя не относится ни к одному из элементов библиотеки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    listBox.ItemsSource = null;
                    listBox.ItemsSource = comicList;
                }
                catch (System.ArgumentException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnRevert_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            DisplayComics();
        }

        public void DisplayComics()
        {
            listBox.ItemsSource = null;
            listBox.ItemsSource = DataBase.GetComics();
        }

        private void BtnReturn_ButtonClickEvent(object sender, RoutedEventArgs e)
        {
            System.Windows.Window mainWindow = System.Windows.Window.GetWindow(this);
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;

            Grid ShowComicsGrid = (Grid)mainWindow.FindName("ShowComicsGrid");
            ShowComicsGrid.Visibility = Visibility.Collapsed;
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
                List<AbstractItem> comics = DataBase.GetComics();
                foreach (AbstractItem item in comics)
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


    }
}
