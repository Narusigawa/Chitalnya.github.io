using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using ClassLibraryEME;
using System.Windows.Media;


namespace LibraryAssignmentWPF.UserControls
{
    public partial class AddBook : UserControl
    {
        Book book = new Book();
        public AddBook()
        {
            InitializeComponent();
            comboBox.ItemsSource = Enum.GetValues(typeof(genre));
        }

        private void btnReturn_Click(object sender, RoutedEventArgs e)
        {
            ReturnToManagerMenu();
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AssignBookProperties();
                IsComboBoxNull();
                book.IsFormValid();
                book.IsIsbnValid(isbnx.txtInput.Text);
                book.IsPriceDouble(pricex.txtInput.Text);
                book.IsQuantityInt(quantityx.txtInput.Text);
                Book actualBook = new Book(isbnx.txtInput.Text, namex.txtInput.Text, editionx.txtInput.Text, book.Quantity, summaryx.txtInput.Text, book.Genre, book.Price);
                DataBase.AddItem(actualBook);
                DataBase.SaveItemInformation(actualBook);
                MessageBox.Show($"{actualBook.Name} успешно создан(а)", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearAllTextBoxes();
                ReturnToManagerMenu();
            }
            catch(ArgumentNullException ex)
            {
                ErrorMessage(ex);
            }
            catch (FormatException ex)
            {
                ErrorMessage(ex);
            }
            catch (ItemAlreadyExistException ex)
            {
                ErrorMessage(ex);
            }
            catch (IllegalIsbnException ex)
            {
                ErrorMessage(ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                ErrorMessage(ex);
                DataBase.RemoveItem(isbnx.txtInput.Text);
            }
        }

        private void ErrorMessage(Exception ex)
        {
            DataBase.LogException(ex);
            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void ClearAllTextBoxes()
        {
            isbnx.txtInput.Clear();
            namex.txtInput.Clear();
            editionx.txtInput.Clear();
            quantityx.txtInput.Clear();
            summaryx.txtInput.Clear();
            pricex.txtInput.Clear();
            comboBox.SelectedItem = null;
            comboBox.Text = "Genre";
            comboBox.Foreground = Brushes.DarkGray;
        }

        private void ReturnToManagerMenu()
        {
            Window mainWindow = Window.GetWindow(this);
            Grid addBookGrid = (Grid)mainWindow.FindName("addBookGrid");
            addBookGrid.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
            ClearAllTextBoxes();
        }

        private void AssignBookProperties()
        {
            book.Isbn = isbnx.txtInput.Text;
            book.Name = namex.txtInput.Text;
            book.Edition = editionx.txtInput.Text;
            book.Summary = summaryx.txtInput.Text;
            book.DateOfPrint = DateTime.Now;
        }

        private void IsComboBoxNull()
        {
            if (comboBox.SelectedItem == null)
            {
                throw new ArgumentNullException("Пожалуйста, выберите жанр!");
            }
            else book.Genre = (genre)comboBox.SelectedItem;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox.Foreground = Brushes.Black;
        }

        private void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            comboBox.Foreground= Brushes.Black;
        }

        private void comboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBox.SelectedItem != null) comboBox.Foreground = Brushes.Black;
            else comboBox.Foreground = Brushes.DarkGray;
        }
    }
}
