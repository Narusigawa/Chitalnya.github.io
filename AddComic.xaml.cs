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
    public partial class AddComic : UserControl
    {
        Comic comic = new Comic();
        public AddComic()
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
                AssignComicProperties();
                IsComboBoxNull();
                comic.IsFormValid();
                comic.IsIsbnValid(isbnx.txtInput.Text);
                comic.IsPriceDouble(pricex.txtInput.Text);
                comic.IsQuantityInt(quantityx.txtInput.Text);
                Comic actualComic = new Comic(isbnx.txtInput.Text, namex.txtInput.Text, editionx.txtInput.Text, comic.Quantity, summaryx.txtInput.Text, comic.Genre, comic.Price);
                DataBase.AddItem(actualComic);
                DataBase.SaveItemInformation(actualComic);
                MessageBox.Show($"{actualComic.Name} успешно создан(а)", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearAllTextBoxes();
                ReturnToManagerMenu();
            }
            catch (ArgumentNullException ex)
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
            Grid addComicGrid = (Grid)mainWindow.FindName("addComicGrid");
            addComicGrid.Visibility = Visibility.Collapsed;
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            managerGrid.Visibility = Visibility.Visible;
            ClearAllTextBoxes();
        }

        private void AssignComicProperties()
        {
            comic.Isbn = isbnx.txtInput.Text;
            comic.Name = namex.txtInput.Text;
            comic.Edition = editionx.txtInput.Text;
            comic.Summary = summaryx.txtInput.Text;
            comic.DateOfPrint = DateTime.Now;
        }

        private void IsComboBoxNull()
        {
            if (comboBox.SelectedItem == null)
            {
                throw new ArgumentNullException("Пожалуйста, выберите жанр!");
            }
            else comic.Genre = (genre)comboBox.SelectedItem;
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox.Foreground = Brushes.Black;
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
    }
}
