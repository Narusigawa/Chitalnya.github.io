using ClassLibraryEME;
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
    public partial class RemoveJournal : UserControl
    {
        Journal journal = new Journal();
        public RemoveJournal()
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
                    journal = (Journal)DataBase.FindItem(isbnBox.txtInput.Text);
                    MessageBoxResult result = MessageBox.Show($"Вы уверены, что хотите удалить журнал: {journal.Name}?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        DataBase.RemoveItem(isbnBox.txtInput.Text);
                        DataBase.DeleteFile(journal);
                        journal = new Journal();
                        MessageBox.Show("Журнал успешно удален!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                    DataBase.AddItem(journal);
                    isbnBox.txtInput.Focus();
                }
                catch (InvalidCastException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show("Введенный ISBN принадлежит книге!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            Grid RemoveJournalGrid = (Grid)mainWindow.FindName("RemoveJournalGrid");
            RemoveJournalGrid.Visibility = Visibility.Collapsed;

            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;

            isbnBox.txtInput.Text = string.Empty;
        }
    }
}
