using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ClassLibraryEME;
using System.Windows.Media;


namespace LibraryAssignmentWPF.UserControls
{
    public partial class EditBook2 : UserControl
    {
        Book book = new Book();
        public EditBook2()
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
                comboBox.Items.Add($"ISBN ({book.Isbn})");
                comboBox.Items.Add($"Название ({book.Name})");
                comboBox.Items.Add($"Издательство ({book.Edition})");
                comboBox.Items.Add($"Количество ({book.Quantity})");
                comboBox.Items.Add($"Цена ({book.Price})");
                comboBox.Items.Add($"Краткое содержание ({book.Summary})");
                comboBox.Items.Add($"Жанр ({book.Genre})");
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
                book = new Book();
                book = (Book)DataBase.FindItem(EditBook.Isbn);
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
            else if(comboBox.SelectedIndex == 5)
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

                viewBoxEnter2.Visibility= Visibility.Visible;
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
            viewBoxCombo.Visibility= Visibility.Visible;

            viewBoxEnter2.Visibility= Visibility.Collapsed;
            viewBoxReturn2.Visibility = Visibility.Collapsed;
            viewBoxTextBox.Visibility = Visibility.Collapsed;

            editTextBox.txtInput.Text = string.Empty;
            comboBox.SelectedItem = null;
            comboBox.Text = "Параметр";
            title.Text = "Пожалуйста, выберите желаемый параметр, который хотите изменить";
        }

        private void HideGenreComboBox()
        {
            viewBoxComboGenre.Visibility= Visibility.Collapsed;
            comboBoxGenre.SelectedItem = null;
            comboBoxGenre.Text = "Жанр";
        }

        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid editBookGrid2 = (Grid)mainWindow.FindName("editBookGrid2");
            editBookGrid2.Visibility= Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
        }

        private void ReturnToFirstMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid editBookGrid2 = (Grid)mainWindow.FindName("editBookGrid2");
            editBookGrid2.Visibility = Visibility.Collapsed;

            Grid editBookGrid = (Grid)mainWindow.FindName("editBookGrid");
            editBookGrid.Visibility = Visibility.Visible;
            comboBox.SelectedValue = null;
            comboBox.Text = "Параметры";
            comboBox.Foreground = Brushes.DarkGray;
        }

        private void EditIsbn()
        {
            string oldIsbn = book.Isbn;
            try
            {
                book.IsIsbnValid(editTextBox.txtInput.Text);
                DataBase.IsIsbnAvailable(editTextBox.txtInput.Text);
                book.Isbn = editTextBox.txtInput.Text;
                DataBase.SaveItemInformation(book);
                MessageBox.Show("ISBN книги был успешно изменен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                book.Isbn = oldIsbn;
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
            if(string.IsNullOrWhiteSpace(editTextBox.txtInput.Text))
            {
                MessageBox.Show("Поле пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldName = book.Name;
                try
                {
                    book.Name = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(book);
                    MessageBox.Show("Название книги было успешно изменено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    book.Name = oldName;
                    editTextBox.txtInput.Focus() ;
                }
            }
        }

        private void EditEdition()
        {
            if(string.IsNullOrWhiteSpace(editTextBox.txtInput.Text))
            {
                MessageBox.Show("Поле пустое!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                editTextBox.txtInput.Focus();
            }
            else
            {
                string oldEdition = book.Edition;
                try
                {
                    book.Edition = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(book);
                    MessageBox.Show("Издание книги было успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    book.Edition = oldEdition;
                    editTextBox.txtInput.Focus();
                }
            }
        }
        
        private void EditQuantity()
        {
            int oldQuantity = book.Quantity;
            try
            {
                book.IsQuantityInt(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(book);
                MessageBox.Show("Количество книг было успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                book.Quantity = oldQuantity;
                editTextBox.txtInput.Focus();
            }
        }

        private void EditPrice()
        {
            double oldPrice = book.Price;
            try
            {
                book.IsPriceDouble(editTextBox.txtInput.Text);
                DataBase.SaveItemInformation(book);
                MessageBox.Show("Цена книг была успешно изменена!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
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
                book.Price = oldPrice;
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
                string oldSummary = book.Summary;
                try
                {
                    book.Summary = editTextBox.txtInput.Text;
                    DataBase.SaveItemInformation(book);
                    MessageBox.Show("Краткое содержание книги было успешно изменено!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    book.Summary = oldSummary;
                    editTextBox.txtInput.Focus();
                }
            }
        }

        private void EditGenre()
        {
            if(comboBoxGenre.SelectedItem != null)
            {
                genre oldGenre = book.Genre;
                try
                {
                    book.Genre = (genre)comboBoxGenre.SelectedItem;
                    DataBase.SaveItemInformation(book);
                    MessageBox.Show("Жанр книги был успешно изменен!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    ReturnToOptionSelectionMenu();
                    ReturnToManagerMenu();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                    book.Genre = oldGenre;
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
