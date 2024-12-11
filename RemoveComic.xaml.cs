using ClassLibraryEME;
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

namespace LibraryAssignmentWPF.UserControls
{
    public partial class RemoveComic : UserControl
    {
        Comic comic = new Comic();
        public RemoveComic()
        {
            InitializeComponent();
            btnEnter.ButtonContent = Resex.btnEnter;
            btnreturn.ButtonContent = Resex.btnReturn;

            btnEnter.ButtonClickEvent += btnInput_Click;
            btnreturn.ButtonClickEvent += btnInput_Click;
        }

        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnEnter)
            {
                try
                {
                    comic = (Comic)DataBase.FindItem(isbnBox.txtInput.Text);
                    MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить комикс: {comic.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        DataBase.RemoveItem(isbnBox.txtInput.Text);
                        DataBase.DeleteFile(comic);
                        comic = new Comic();
                        MessageBox.Show("Комикс успешно удален!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                        ReturnToManagerMenu();
                    }
                }
                catch (IllegalIdException ex)
                {
                    ErrorMessage(ex);
                    isbnBox.txtInput.Focus();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    DataBase.AddItem(comic);
                    isbnBox.txtInput.Focus();
                }
                catch (InvalidCastException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show("Введенный ISBN принадлежит журналу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ReturnToManagerMenu();
            }
        }

        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid RemoveComicGrid = (Grid)mainWindow.FindName("RemoveComicGrid");
            RemoveComicGrid.Visibility = Visibility.Collapsed;

            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;

            isbnBox.txtInput.Text = string.Empty;
        }
    }
}
