using ClassLibraryEME;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ClassLibraryEME
{
    public static class DataBase
    {
        static List<AbstractItem> libraryItems = new List<AbstractItem>();
        static string dataBase = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase"; //изменить путь

        public static void SaveItemInformation(AbstractItem item)
        {
            CheckForDbExistence();
            string bookDirectory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Books";
            if (!Directory.Exists(bookDirectory)) throw new DirectoryNotFoundException("Не удалось найти каталог базы данных книг");
            string journalDirectory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Journals";
            if (!Directory.Exists(journalDirectory)) throw new DirectoryNotFoundException("Не удалось найти каталог базы данных журналов");
            string comicDirectory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Comics";
            if (!Directory.Exists(comicDirectory)) throw new DirectoryNotFoundException("Не удалось найти каталог базы данных комиксов");

            string directory = "";
            string fileName = "";

            if (item.GetType() == typeof(Book))
            {
                directory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Books\Book" + " " + item.Isbn;
                fileName = "BookInfo.json";
                Directory.CreateDirectory(directory);
                string filePath = Path.Combine(directory, fileName);
                string itemSerialized = JsonSerializer.Serialize((Book)item);
                File.WriteAllText(filePath, itemSerialized);
            }
            else if (item.GetType() == typeof(Comic))
            {
                directory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Comics\Comic" + " " + item.Isbn;
                fileName = "ComicInfo.json";
                Directory.CreateDirectory(directory);
                string filePath = Path.Combine(directory, fileName);
                string itemSerialized = JsonSerializer.Serialize((Comic)item);
                File.WriteAllText(filePath, itemSerialized);
            }
            else
            {
                directory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Journals\Journal" + " " + item.Isbn;
                fileName = "JouralInfo.json";
                Directory.CreateDirectory(directory);
                string filePath = Path.Combine(directory, fileName);
                string itemSerialized = JsonSerializer.Serialize((Journal)item);
                File.WriteAllText(filePath, itemSerialized);
            }
        }
        public static void LoadItems()
        {
            CheckForDbExistence();

            string directoryPathBooks = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Books";
            string directoryPathJournals = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Journals";
            string directoryPathComics = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Comics";


            if (Directory.Exists(directoryPathBooks))
            {
                foreach (string filePath in Directory.GetFiles(directoryPathBooks, "BookInfo.json", SearchOption.AllDirectories))
                {
                    string txtString = File.ReadAllText(filePath);
                    Book book = JsonSerializer.Deserialize<Book>(txtString);
                    AddItem(book);
                }
            }
            else throw new DirectoryNotFoundException("Не удалось найти каталог книг");

            if (Directory.Exists(directoryPathJournals))
            {
                foreach (string filePath in Directory.GetFiles(directoryPathJournals, "JournalInfo.json", SearchOption.AllDirectories))
                {
                    string txtString = File.ReadAllText(filePath);
                    Journal journal = JsonSerializer.Deserialize<Journal>(txtString);
                    AddItem(journal);
                }
            }
            else throw new DirectoryNotFoundException("Не удалось найти каталог журналов");

            if (Directory.Exists(directoryPathComics))
            {
                foreach (string filePath in Directory.GetFiles(directoryPathComics, "ComicInfo.json", SearchOption.AllDirectories))
                {
                    string txtString = File.ReadAllText(filePath);
                    Comic comic = JsonSerializer.Deserialize<Comic>(txtString);
                    AddItem(comic);
                }
            }
            else throw new DirectoryNotFoundException("Не удалось найти каталог комиксов");
        }
        public static void DeleteFile(AbstractItem item)
        {
            string directory = "";
            if (item.GetType() == typeof(Book))
            {
                directory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Books\Book" + " " + item.Isbn;
            }
            else if (item.GetType() == typeof(Comic))
            {
                directory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Comics\Comic" + " " + item.Isbn;
            }
            else
            {
                directory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Journals\Journal" + " " + item.Isbn;
            }
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
            else throw new DirectoryNotFoundException("Директория не найдена");
        }
        public static void SaveReceipt(List<AbstractItem> items, double totalPrice, bool ClubDiscount = false)
        {
            CheckForDbExistence();

            string itemReceiptsDirectory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\ItemReceipts";
            if (!Directory.Exists(itemReceiptsDirectory)) throw new DirectoryNotFoundException("Директория рецептов не найдена!");

            string directory = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\ItemReceipts\Receipt" + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            string fileName = $"Receipt.txt";

            Directory.CreateDirectory(directory);
            string filePath = Path.Combine(directory, fileName);

            StringBuilder receipt = new StringBuilder();
            receipt.AppendLine(DateTime.Now.ToString());
            receipt.AppendLine("Купленные товары:");

            foreach (var item in items)
            {
                string itemInfo = item.PartialToString();
                receipt.AppendLine(itemInfo);
            }
            if (ClubDiscount == false) receipt.AppendLine($"Итоговая цена: {totalPrice:C}");
            else receipt.AppendLine($"Итоговая цена с скидкой клуба: {totalPrice:C}");

            File.WriteAllText(filePath, receipt.ToString());
        }
        public static List<string> GetReceipts()
        {
            CheckForDbExistence();

            var receipts = new List<string>();
            string directoryPath = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\ItemReceipts";

            if (Directory.Exists(directoryPath))
            {
                foreach (string filePath in Directory.GetFiles(directoryPath, "Receipt.txt", SearchOption.AllDirectories))
                {
                    string receipt = File.ReadAllText(filePath);
                    receipts.Add(receipt);
                }
                return receipts;
            }
            else throw new DirectoryNotFoundException("Не удалось найти каталог квитанций");
        }
        public static List<string> GetExceptions()
        {
            CheckForDbExistence();

            var exceptions = new List<string>();
            string directoryPath = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Exceptions";

            if (Directory.Exists(directoryPath))
            {
                foreach (string filePath in Directory.GetFiles(directoryPath, "exception.txt", SearchOption.AllDirectories))
                {
                    string receipt = File.ReadAllText(filePath);
                    exceptions.Add(receipt);
                }
                return exceptions;
            }
            else throw new DirectoryNotFoundException("Не удалось найти каталог исключений");
        }
        public static List<string> GetCustomers()
        {
            CheckForDbExistence();

            var customers = new List<string>();
            string directoryPath = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Customers";

            if (Directory.Exists(directoryPath))
            {
                foreach (string filePath in Directory.GetFiles(directoryPath, "CustomerId.txt", SearchOption.AllDirectories))
                {
                    string receipt = File.ReadAllText(filePath);
                    customers.Add(receipt);
                }
                return customers;
            }
            else throw new DirectoryNotFoundException("Не удалось найти каталог клиентов");
        }

        public static AbstractItem GetItemByIndex(int index)
        {
            return libraryItems[index]; //возможно изменить на classlibraryEME
        }

        public static void LogException(Exception exception)
        {
            if (!Directory.Exists(dataBase)) return;

            string directoryPath = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Exceptions\Exception " + exception.GetType().Name + " " + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
            string fileName = "exception.txt";
            string directoryPathExceptions = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Exceptions";
            if (Directory.Exists(directoryPathExceptions))
            {
                Directory.CreateDirectory(directoryPath);
                string filePath = Path.Combine(directoryPath, fileName);
                string exceptionText = $"Exception: {exception} \nDate: {DateTime.Now}";
                File.WriteAllText(filePath, exceptionText);
            }
            else return;
        }

        public static void DoesReceiptsDirExist()
        {
            CheckForDbExistence();

            string directoryPath = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\ItemReceipts";
            if (Directory.Exists(directoryPath))
            {
                int length = Directory.GetDirectories(directoryPath).Length;
                if (length == 0)
                {
                    throw new DirectoryNotFoundException("В настоящее время квитанции недоступны для отображения!");
                }
            }
            else throw new DirectoryNotFoundException("Не удалось найти каталог квитанций!");
        }

        public static void DoesExceptionsDirExist()
        {
            CheckForDbExistence();

            string directoryPath = @"C:\Users\user\Desktop\ClassLibraryEME — копия — копия\DataBase\Exceptions";
            if (Directory.Exists(directoryPath))
            {
                int length = Directory.GetDirectories(directoryPath).Length;
                if (length == 0)
                {
                    throw new DirectoryNotFoundException("В настоящее время исключений недоступны для отображения!");
                }
            }
            else throw new DirectoryNotFoundException("Не удалось найти каталог исключений!");
        }

        public static void CheckForDbExistence()
        {
            if (!Directory.Exists(dataBase)) throw new DirectoryNotFoundException("Не удалось найти директорию базы данных");
        }

        public static List<AbstractItem> FilterItemsByName(string name)
        {
            var itemsFoundWithName = new List<AbstractItem>();
            foreach (var item in libraryItems)
            {
                if (HasContinuousSubstring(item.Name, name)) itemsFoundWithName.Add(item);
            }

            if (itemsFoundWithName.Count == 0) throw new ArgumentException("Введенное вами имя не относится ни к одному из элементов библиотеки!");
            return itemsFoundWithName;
        }
        public static bool HasContinuousSubstring(string itemName, string name)
        {
            int nameIndex = 0;
            int counter = 0;
            for (int i = 0; i < itemName.Length; i++)
            {
                if (counter == name.Length) return true;
                if (name[nameIndex] == itemName[i])
                {
                    counter++;
                    nameIndex++;
                }
                else
                {
                    counter = 0;
                    nameIndex = 0;
                }
            }
            return counter == name.Length;
        }

        public static List<AbstractItem> GetItems()
        {
            return libraryItems; //EME
        }

        public static List<AbstractItem> GetBooks()
        {
            return libraryItems.Where(Item => Item is Book).ToList();
        }
        public static List<AbstractItem> GetJournals()
        {
            return libraryItems.Where(Item => Item is Journal).ToList(); //EME
        }

        public static List<AbstractItem> GetComics()
        {
            return libraryItems.Where(Item => Item is Comic).ToList();
        }
        public static int ItemCount()
        {
            return libraryItems.Count; //EME
        }
        public static int BookCount()
        {
            int count = 0;
            foreach (var item in libraryItems)
            {
                if (item.GetType() == typeof(Book)) count++;
            }
            return count;
        }
        public static int JournalCount()
        {
            int count = 0;
            foreach (var item in libraryItems)
            {
                if (item.GetType() == typeof(Journal)) count++;
            }
            return count;
        }
        public static int ComicCount()
        {
            int count = 0;
            foreach (var item in libraryItems)
            {
                if (item.GetType() == typeof(Comic)) count++;
            }
            return count;
        }

        public static void AddItem(AbstractItem item)
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == item.Isbn)
                {
                    throw new ItemAlreadyExistException("Этот элемент уже существует в библиотеке!");
                }
            }
            libraryItems.Add(item);
        }
        public static void IsIsbnAvailable(string isbn)
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    throw new IllegalIsbnException("Этот ISBN уже существует в библиотеке!");
                }
            }
        }
        public static void IsIsbnValid(string isbn)
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    return;
                }
            }
            throw new IllegalIsbnException("Введенный ISBN не соответствует существующему товару!");
        }
        public static AbstractItem FindItem(string isbn)
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    return existingItem;
                }
            }
            throw new IllegalIsbnException("Введенный ISBN не соответствует существующему товару!");
        }

        public static void RemoveItem(string isbn)
        {
            foreach (var existingItem in libraryItems)
            {
                if (existingItem.Isbn == isbn)
                {
                    libraryItems.Remove(existingItem);
                    return;
                }
            }
            throw new IllegalIsbnException("Введенный ISBN не соответствует существующему товару!");
        }
    }
}
