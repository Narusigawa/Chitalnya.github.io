using System.Text.RegularExpressions;
namespace ClassLibraryEME
{
    public abstract class AbstractItem
    {
        public string Isbn { get; set; }
        public string Name { get; set; }
        public string Edition { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public DateTime DateOfPrint { get; set; }

        public AbstractItem(string isbn, string name, string edition, int quantity, double price) 
        {
            Isbn = isbn;
            Name = name;
            Edition = edition;
            Quantity = quantity;
            Price = price;
            DateOfPrint = DateTime.Now;
        }
        public AbstractItem()
        {

        }
        public abstract void IsFormValid();
        public void IsPriceDouble(string price)
        {
            if (!double.TryParse(price, out double parsedPrice) || parsedPrice <= 0 || parsedPrice > 9999)
            {
                throw new FormatException("Цена должна быть положительной и не больше 9999!");
            }
            Price = parsedPrice;
        }
        public void IsQuantityInt(string quantity)
        {
            if (!int.TryParse(quantity, out int parsedQuantity) || parsedQuantity < 1 || parsedQuantity > 500)
            {
                throw new FormatException("Количество должно быть положительным числом и не больше 500!");
            }
            Quantity = parsedQuantity;
        }
        public void IsIsbnValid(string isbn)
        {
            string isbnPattern = @"^\d{6}$";
            Regex IsIsbnValid = new Regex(isbnPattern);

            if (!IsIsbnValid.IsMatch(isbn)) throw new IllegalIsbnException("ISBN должен содержать ровно 6 чисел");
        }
        public void SubstractItemFromLibrary()
        {
            if (Quantity == 0)
            {
                throw new ItemOutOfStockExceptions($"{Name} отсутсвует или уже существует!");
            }
            Quantity--;
        }
        public string PartialToString()
        {
            if (GetType() == typeof(Book))
            {
                return $"ISBN: {Isbn} | Название: {Name} | Цена: {Price:C} | Книга";
            }
            else if (GetType() == typeof(Comic))
            {
                return $"ISBN: {Isbn} | Название: {Name} | Цена: {Price:C} | Комикс";
            }
            else
            {
                return $"ISBN: {Isbn} | Название: {Name} | Цена: {Price:C} | Журнал";
            }
        }


    }
}
