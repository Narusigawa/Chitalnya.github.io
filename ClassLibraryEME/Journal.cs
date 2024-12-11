using ClassLibraryEME;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibraryEME
{
    public class Journal : AbstractItem
    {
        public Journal(string Isbn, string Name, string Edition, int Quantity, double Price)
            : base(Isbn, Name, Edition, Quantity, Price)
        {

        }

        public Journal() : base()
        {

        }

        public override void IsFormValid()
        {
            if (String.IsNullOrWhiteSpace(Isbn) ||
                String.IsNullOrWhiteSpace(Name) ||
                String.IsNullOrWhiteSpace(Edition) ||
                String.IsNullOrWhiteSpace(Price.ToString()) ||
                String.IsNullOrWhiteSpace(Quantity.ToString()))
            {
                throw new ArgumentNullException("Одно или несколько полей не заданы!");
            }
        }
        public override string ToString()
        {
            return $"Журнал | ISBN: {Isbn} | Название: {Name} | Дата создания {DateOfPrint} | Издательство: {Edition} |" +
                $" Количество в магазине: {Quantity} | Цена: {Price:C}";
        }
    }
}
