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
    public partial class EditJournal2 : UserControl
    {
        Journal journal = new Journal();
        public EditJournal2()
        {
            InitializeComponent();

            btnEnter.ButtonContent = Resex.btnEnter;
            btnReturn.ButtonContent = Resex.btnReturn;

            btnEnter.ButtonClickEvent += btnInput_Click;
            btnReturn.ButtonClickEvent += btnInput_Click;
        }

        public void PopulateComboBox()
        {
            if (comboBox.Items.Count == 0)
            {
                comboBox.Items.Add($"ISBN ({journal.Isbn})");
                comboBox.Items.Add($"Название ({journal.Name})");
                comboBox.Items.Add($"Издательство ({journal.Edition})");
                comboBox.Items.Add($"Количество ({journal.Quantity})");
                comboBox.Items.Add($"Цена ({journal.Price})");
            }
        }

        public void ClearComboBox()
        {
            comboBox.Items.Clear();
        }

        public void FindJournal()
        {
            try
            {
                journal = new Journal();
                journal = (Journal)DataBase.FindItem(EditJournal.Isbn!);
            }
            catch (InvalidCastException ex)
            {
                DataBase.LogException(ex);
                ReturnToFirstMenu();
                MessageBox.Show("ISBN, который вы ввели, принадлежит другому товару!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnInput_Click(object sender, RoutedEventArgs e)
        {
            if (sender == btnEnter)
            {
                if (comboBox.SelectedItem != null)
                {
                    ProceedToChangeProperty();
                }
                else
                {
                    MessageBox.Show("Пожалуйста, выберите параметр!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                ReturnToFirstMenu();
            }
        }

        private void btnEnter2_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedIndex == 0)
            {
                EditIsbn();
            }
            else if (comboBox.SelectedIndex == 1)
            {
                EditName();
            }
            else if (comboBox.SelectedIndex == 2)
            {
                EditEdition();
            }
            else if (comboBox.SelectedIndex == 3)
            {
                EditQuantity();
            }
            else
            {
                EditPrice();
            }
        }
        private void btnReturn2_Click(object sender, RoutedEventArgs e)
        {
            ReturnToOptionSelectionMenu();
        }
        private void ProceedToChangeProperty()
        {
            btnEnter.Visibility = Visibility.Collapsed;
            btnReturn.Visibility = Visibility.Collapsed;
            viewBoxCombo.Visibility = Visibility.Collapsed;

            viewBoxEnter2.Visibility = Visibility.Visible;
            viewBoxReturn2.Visibility = Visibility.Visible;
            viewBoxTextBox.Visibility = Visibility.Visible;

            string boundText = "";
            for (int i = 0; i < comboBox.SelectedItem.ToString()!.Length; i++)
            {
                if (Char.IsLetter(comboBox.SelectedItem.ToString()![i])) boundText += comboBox.SelectedItem.ToString()![i];
                else break;
            }
            editTextBox.BoundText = $"Введите новый {boundText}";
            title.Text = $"Пожалуйста, выберите новый {boundText}";
        }

        private void ReturnToOptionSelectionMenu()
        {
            btnEnter.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Visible;
            viewBoxCombo.Visibility = Visibility.Visible;

            viewBoxEnter2.Visibility = Visibility.Collapsed;
            viewBoxReturn2.Visibility = Visibility.Collapsed;
            editTextBox.txtInput.Text = string.Empty;
            title.Text = "Пожалуйста, выберите желаемый параметр, который хотите изменить:";
        }
        private void ReturnToManageMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditJournalGrid2 = (Grid)mainWindow.FindName("EditJournalGrid2");
            EditJournalGrid2.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
        }

        private void ReturnToFirstMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid EditJournalGrid2 = (Grid)mainWindow.FindName("EditJournalGrid2");
            EditJournalGrid2.Visibility = Visibility.Collapsed;

            Grid EditJournalGrid = (Grid)mainWindow.FindName("EditJournalGrid");
            EditJournalGrid.Visibility = Visibility.Visible;
            comboBox.SelectedValue = null;
            comboBox.Text = "Параметр";
            comboBox.Foreground = Brushes.DarkGray;
        }
        private void EditIsbn()
        {
            string oldIsbn = journal.Isbn;
            try
            {
                journal.IsIsbnValid(editTextBox.txtInput.Text);
                DataBase.IsIsbnAvailable(editTextBox.txtInput.Text);
                journal.Isbn = editTextBox.txtInput.Text;
                DataBase.SaveItemInformation(journal);
                MessageBox.Show("ISBN журнала был успешно изменен!!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManageMenu();
            }
            catch (IllegalIdException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                journal.Isbn = oldIsbn;
                editTextBox.txtInput.Focus();
            }
        }
        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void EditName()
        {
            if (string.IsNullOrWhiteSpace(editTextBox.txtInput.Text))
            {
                MessageBox.Show("Файл пустой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldName = journal.Name;
                try
                {
                    journal.Name = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(journal);
                    MessageBox.Show("Название журнала было успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManageMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    journal.Name = oldName;
                    editTextBox.txtInput.Focus();
                }
            }
        }
        private void EditEdition()
        {
            if (string.IsNullOrWhiteSpace(editTextBox.txtInput.Text))
            {
                MessageBox.Show("Файл пустой!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldEdition = journal.Edition;
                try
                {
                    journal.Edition = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(journal);
                    MessageBox.Show("Издательство журнала было успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManageMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    ErrorMessage(ex);
                    journal.Edition = oldEdition;
                    editTextBox.txtInput.Focus();
                }
            }
        }
        private void EditQuantity()
        {
            int oldQuantity = journal.Quantity;
            try
            {
                journal.IsQuantityInt(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(journal);
                MessageBox.Show("Количество журналов успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManageMenu();
            }
            catch (FormatException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                journal.Quantity = oldQuantity;
                editTextBox.txtInput.Focus();
            }
        }
        private void EditPrice()
        {
            double oldPrice = journal.Price;
            try
            {
                journal.IsPriceDouble(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(journal);
                MessageBox.Show("Цена журнала была успешно изменена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManageMenu();
            }
            catch (FormatException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                journal.Price = oldPrice;
                editTextBox.txtInput.Focus();
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox.Foreground = Brushes.Black;
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem != null) comboBox.Foreground = Brushes.Black;
            else comboBox.Foreground = Brushes.DarkGray;
        }

        private void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            comboBox.Foreground = Brushes.Black;
        }
    }
}