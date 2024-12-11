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
    public partial class EditComic2 : UserControl
    {
        Comic comic = new Comic();
        public EditComic2()
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
                comboBox.Items.Add($"ISBN ({comic.Isbn})");
                comboBox.Items.Add($"Навзание ({comic.Name})");
                comboBox.Items.Add($"Издательство ({comic.Edition})");
                comboBox.Items.Add($"Количество ({comic.Quantity})");
                comboBox.Items.Add($"Цена ({comic.Price})");
                comboBox.Items.Add($"Краткое содержание ({comic.Summary})");
                comboBox.Items.Add($"Жанр ({comic.Genre})");
            }
        }

        public void ClearComboBox()
        {
            comboBox.Items.Clear();
        }

        public void FindBook()
        {
            try
            {
                comic = new Comic();
                comic = (Comic)DataBase.FindItem(EditComic.Isbn);
            }
            catch (InvalidCastException ex)
            {
                DataBase.LogException(ex);
                ReturnToFirstMenu();
                MessageBox.Show("Введенный ISBN принадлежит журналу!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    MessageBox.Show("Пожалуйста, выберите параметр", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            else if (comboBox.SelectedIndex == 4)
            {
                EditPrice();
            }
            else if (comboBox.SelectedIndex == 5)
            {
                EditSummary();
            }
            else
            {
                EditGenre();
            }
        }

        private void btnReturn2_Click(object sender, RoutedEventArgs e)
        {
            ReturnToOptionSelectionMenu();
        }

        private void ProceedToChangeProperty()
        {
            if (comboBox.SelectedIndex == 6)
            {
                btnEnter.Visibility = Visibility.Collapsed;
                btnReturn.Visibility = Visibility.Collapsed;
                viewBoxCombo.Visibility = Visibility.Collapsed;

                viewBoxEnter2.Visibility = Visibility.Visible;
                viewBoxReturn2.Visibility = Visibility.Visible;
                viewBoxComboGenre.Visibility = Visibility.Visible;

                comboBoxGenre.ItemsSource = Enum.GetValues(typeof(genre));
                title.Text = $"Пожалуйста, выберите новый жанр";
            }
            else
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
        }

        private void ReturnToOptionSelectionMenu()
        {
            if (comboBox.SelectedIndex == 6)
            {
                HideGenreComboBox();
            }
            btnEnter.Visibility = Visibility.Visible;
            btnReturn.Visibility = Visibility.Visible;
            viewBoxCombo.Visibility = Visibility.Visible;

            viewBoxEnter2.Visibility = Visibility.Collapsed;
            viewBoxReturn2.Visibility = Visibility.Collapsed;
            viewBoxTextBox.Visibility = Visibility.Collapsed;

            editTextBox.txtInput.Text = string.Empty;
            comboBox.SelectedItem = null;
            comboBox.Text = "Параметр";
            title.Text = "Пожалуйста, выберите желаемый параметр, который хотите изменить";
        }

        private void HideGenreComboBox()
        {
            viewBoxComboGenre.Visibility = Visibility.Collapsed;
            comboBoxGenre.SelectedItem = null;
            comboBoxGenre.Text = "Genre";
        }

        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid editComicGrid2 = (Grid)mainWindow.FindName("editComicGrid2");
            editComicGrid2.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
        }

        private void ReturnToFirstMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid editComicGrid2 = (Grid)mainWindow.FindName("editComicGrid2");
            editComicGrid2.Visibility = Visibility.Collapsed;

            Grid editComicGrid = (Grid)mainWindow.FindName("editComicGrid");
            editComicGrid.Visibility = Visibility.Visible;
            comboBox.SelectedValue = null;
            comboBox.Text = "Параметры";
            comboBox.Foreground = Brushes.DarkGray;
        }

        private void EditIsbn()
        {
            string oldIsbn = comic.Isbn;
            try
            {
                comic.IsIsbnValid(editTextBox.txtInput.Text);
                DataBase.IsIsbnAvailable(editTextBox.txtInput.Text);
                comic.Isbn = editTextBox.txtInput.Text;
                DataBase.SaveItemInformation(comic);
                MessageBox.Show("ISBN комикса был успешно изменен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManagerMenu();
            }
            catch (IllegalIsbnException ex)
            {
                ErrorMessage(ex);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                comic.Isbn = oldIsbn;
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
                MessageBox.Show("Поле пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldName = comic.Name;
                try
                {
                    comic.Name = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(comic);
                    MessageBox.Show("Название комикса было успешно изменено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    comic.Name = oldName;
                    editTextBox.txtInput.Focus();
                }
            }
        }

        private void EditEdition()
        {
            if (string.IsNullOrWhiteSpace(editTextBox.txtInput.Text))
            {
                MessageBox.Show("Поле пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldEdition = comic.Edition;
                try
                {
                    comic.Edition = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(comic);
                    MessageBox.Show("Издание комикса было успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    comic.Edition = oldEdition;
                    editTextBox.txtInput.Focus();
                }
            }
        }

        private void EditQuantity()
        {
            int oldQuantity = comic.Quantity;
            try
            {
                comic.IsQuantityInt(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(comic);
                MessageBox.Show("Количество комиксов было успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManagerMenu();
            }
            catch (FormatException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                comic.Quantity = oldQuantity;
                editTextBox.txtInput.Focus();
            }
        }

        private void EditPrice()
        {
            double oldPrice = comic.Price;
            try
            {
                comic.IsPriceDouble(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(comic);
                MessageBox.Show("Цена комиксов была успешно изменена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                ReturnToOptionSelectionMenu();
                ReturnToManagerMenu();
            }
            catch (FormatException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                editTextBox.txtInput.Focus();
            }
            catch (DirectoryNotFoundException ex)
            {
                DataBase.LogException(ex);
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                comic.Price = oldPrice;
                editTextBox.txtInput.Focus();
            }
        }

        private void EditSummary()
        {
            if (string.IsNullOrWhiteSpace(editTextBox.txtInput.Text))
            {
                MessageBox.Show("Поле пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldSummary = comic.Summary;
                try
                {
                    comic.Summary = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(comic);
                    MessageBox.Show("Краткое содержание комикса было успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    comic.Summary = oldSummary;
                    editTextBox.txtInput.Focus();
                }
            }
        }

        private void EditGenre()
        {
            if (comboBoxGenre.SelectedItem != null)
            {
                genre oldGenre = comic.Genre;
                try
                {
                    comic.Genre = (genre)comboBoxGenre.SelectedItem;
                    DataBase.SaveItemInformation(comic);
                    MessageBox.Show("Жанр комикса был успешно изменен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    comic.Genre = oldGenre;
                    editTextBox.txtInput.Focus();
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите жанр!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            comboBox.Foreground = Brushes.Black;
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem != null) comboBox.Foreground = Brushes.Black;
            else comboBox.Foreground = Brushes.DarkGray;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox.Foreground = Brushes.Black;
        }

        private void comboBoxGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBoxGenre.Foreground = Brushes.Black;
        }

        private void comboBoxGenre_DropDownOpened(object sender, EventArgs e)
        {
            comboBoxGenre.Foreground = Brushes.Black;
        }

        private void comboBoxGenre_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem != null) comboBox.Foreground = Brushes.Black;
            else comboBox.Foreground = Brushes.DarkGray;
        }
    }
}
