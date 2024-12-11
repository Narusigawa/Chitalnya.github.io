using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using ClassLibraryEME;

namespace LibraryAssignmentWPF.UserControls
{
    public partial class Manager : UserControl
    {
        public Manager()
        {
            InitializeComponent();
            AssignNamesToButton();
            AssignEventToButton();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window mainWindow = Window.GetWindow(this);
            Grid managerGrid = (Grid)mainWindow.FindName("managerGrid");
            if (sender == addBook)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid addBookGrid = (Grid)mainWindow.FindName("addBookGrid");
                addBookGrid.Visibility = Visibility.Visible;
            }
            else if (sender == addJournal)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid addJournalGrid = (Grid)mainWindow.FindName("AddJournalGrid");
                addJournalGrid.Visibility = Visibility.Visible;
            }
            else if (sender == addComic)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid addComicGrid = (Grid)mainWindow.FindName("addComicGrid");
                addComicGrid.Visibility = Visibility.Visible;
            }
            else if (sender == editBook)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid editBookGrid = (Grid)mainWindow.FindName("editBookGrid");
                editBookGrid.Visibility = Visibility.Visible;
            }
            else if (sender == editJournal)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid editJournalGrid = (Grid)mainWindow.FindName("EditJournalGrid");
                editJournalGrid.Visibility = Visibility.Visible;
            }
            else if (sender == editComic)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid editComicGrid = (Grid)mainWindow.FindName("editComicGrid");
                editComicGrid.Visibility = Visibility.Visible;
            }
            else if (sender == removeBook)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid RemoveBookGrid = (Grid)mainWindow.FindName("RemoveBookGrid");
                RemoveBookGrid.Visibility = Visibility.Visible;
            }
            else if (sender == removeComic)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid RemoveComicGrid = (Grid)mainWindow.FindName("RemoveComicGrid");
                RemoveComicGrid.Visibility = Visibility.Visible;
            }
            else if (sender == removeJournal)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid RemoveJournalGrid = (Grid)mainWindow.FindName("RemoveJournalGrid");
                RemoveJournalGrid.Visibility = Visibility.Visible;
            }
            else if (sender == showAvailableBooks)
            {
                int bookCount = DataBase.BookCount();
                if (bookCount == 0)
                {
                    MessageBox.Show("В настоящее время нет книг, доступных для показа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowBooksGrid = (Grid)mainWindow.FindName("ShowBooksGrid");
                    ShowBooksGrid.Visibility = Visibility.Visible;

                    ShowBooks showBooksControl = (ShowBooks)ShowBooksGrid.Children[0];
                    showBooksControl.DisplayBooks();
                }
            }
            else if (sender == showAvailableJournals)
            {
                int journalCount = DataBase.JournalCount();
                if (journalCount == 0)
                {
                    MessageBox.Show("В настоящее время нет журналов, доступных для показа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowJournalsGrid = (Grid)mainWindow.FindName("ShowJournalsGrid");
                    ShowJournalsGrid.Visibility = Visibility.Visible;

                    ShowJournals showJournalsControl = (ShowJournals)ShowJournalsGrid.Children[0];
                    showJournalsControl.DisplayJournals();
                }
            }
            else if (sender == showAvailableComics)
            {
                int comicCount = DataBase.ComicCount();
                if (comicCount == 0)
                {
                    MessageBox.Show("В настоящее время нет комиксов, доступных для показа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowComicsGrid = (Grid)mainWindow.FindName("ShowComicsGrid");
                    ShowComicsGrid.Visibility = Visibility.Visible;

                    ShowComics showComicsControl = (ShowComics)ShowComicsGrid.Children[0];
                    showComicsControl.DisplayComics();
                }
            }
            else if (sender == showAvailableItems)
            {
                int itemCount = DataBase.ItemCount();
                if (itemCount == 0)
                {
                    MessageBox.Show("В настоящее время нет товаров, доступных для показа", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowAvailableItemsGrid = (Grid)mainWindow.FindName("ShowAvailableItemsGrid");
                    ShowAvailableItemsGrid.Visibility = Visibility.Visible;

                    ShowAvailableItems ShowAvailableItemsControl = (ShowAvailableItems)ShowAvailableItemsGrid.Children[0];
                    ShowAvailableItemsControl.DisplayItems();
                }
            }
            else if (sender == managePurchases)
            {
                try
                {
                    DataBase.DoesReceiptsDirExist();
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ReceiptsGrid = (Grid)mainWindow.FindName("ReceiptsGrid");
                    ReceiptsGrid.Visibility = Visibility.Visible;
                    Receipts ReceiptsControl = (Receipts)ReceiptsGrid.Children[0];
                    ReceiptsControl.DisplayReceipts();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (sender == manageExceptions)
            {
                try
                {
                    DataBase.DoesExceptionsDirExist();
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ManageExceptionsGrid = (Grid)mainWindow.FindName("ManageExceptionsGrid");
                    ManageExceptionsGrid.Visibility = Visibility.Visible;
                    ManageException ManageExceptionsControl = (ManageException)ManageExceptionsGrid.Children[0]; //s
                    ManageExceptionsControl.DisplayExceptions();
                }
                catch (DirectoryNotFoundException ex)
                {
                    DataBase.LogException(ex);
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (sender == clubCustomers)
            {
                int customerCount = Customer.GetCustomersCount();
                if (customerCount == 0)
                {
                    MessageBox.Show("В настоящее время в клубе нет посетителей!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    managerGrid.Visibility = Visibility.Collapsed;
                    Grid ShowClubCustomersGrid = (Grid)mainWindow.FindName("ShowClubCustomersGrid");
                    ShowClubCustomersGrid.Visibility = Visibility.Visible;

                    ShowClubCustomers ShowClubCustomersControl = (ShowClubCustomers)ShowClubCustomersGrid.Children[0];
                    ShowClubCustomersControl.DisplayCustomersIds();
                }
            }
            else if (sender == mainmenu)
            {
                managerGrid.Visibility = Visibility.Collapsed;
                Grid mainMenu = (Grid)mainWindow.FindName("MainMenu");
                mainMenu.Visibility = Visibility.Visible;
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
        private void AssignEventToButton()
        {
            addBook.ButtonClickEvent += Button_Click;
            addJournal.ButtonClickEvent += Button_Click;
            addComic.ButtonClickEvent += Button_Click;
            editBook.ButtonClickEvent += Button_Click;
            editJournal.ButtonClickEvent += Button_Click;
            editComic.ButtonClickEvent += Button_Click;
            removeBook.ButtonClickEvent += Button_Click;
            removeJournal.ButtonClickEvent += Button_Click;
            removeComic.ButtonClickEvent += Button_Click;
            showAvailableBooks.ButtonClickEvent += Button_Click;
            showAvailableJournals.ButtonClickEvent += Button_Click;
            showAvailableItems.ButtonClickEvent += Button_Click;
            showAvailableComics.ButtonClickEvent += Button_Click;
            managePurchases.ButtonClickEvent += Button_Click;
            manageExceptions.ButtonClickEvent += Button_Click;
            clubCustomers.ButtonClickEvent += Button_Click;
            mainmenu.ButtonClickEvent += Button_Click;
            exit.ButtonClickEvent += Button_Click;
        }

        private void AssignNamesToButton()
        {
            addBook.ButtonContent = Resex.btnAddBook;
            addJournal.ButtonContent = Resex.btnAddJournal;
            addComic.ButtonContent = Resex.btnAddComic;
            editBook.ButtonContent = Resex.btnEditBook;
            editJournal.ButtonContent = Resex.btnEditJournal;
            editComic.ButtonContent = Resex.btnEditComic;
            removeBook.ButtonContent = Resex.btnRemoveBook;
            removeJournal.ButtonContent = Resex.btnRemoveJournal;
            removeComic.ButtonContent = Resex.btnRemoveComic;
            showAvailableBooks.ButtonContent = Resex.btnShowBooks;
            showAvailableJournals.ButtonContent = Resex.btnShowJournals;
            showAvailableComics.ButtonContent = Resex.btnShowComics;
            showAvailableItems.ButtonContent = Resex.btnShowItems;
            managePurchases.ButtonContent = Resex.btnPurcchases;
            manageExceptions.ButtonContent = Resex.btnManageException;
            clubCustomers.ButtonContent = Resex.btnClubCustomers;
            mainmenu.ButtonContent = Resex.btnMainMenu;
            exit.ButtonContent = Resex.btnExit;
        }
    }
}
