using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibraryEME
{
    public class Comic : AbstractItem
    {
        public string? Summary { get; set; }
        public genre Genre { get; set; }
        public Comic(string isbn, string name, string edition, int Quantity, string? summary, genre genre, double Price)
            : base(isbn, name, edition, Quantity, Price)
        {
            Summary = summary;
            Genre = genre;
        }
        public Comic()
        {

        }
        public override void IsFormValid()
        {
            if (String.IsNullOrWhiteSpace(Isbn) ||
                String.IsNullOrWhiteSpace(Name) ||
                String.IsNullOrWhiteSpace(Edition) ||
                String.IsNullOrWhiteSpace(Quantity.ToString()) ||
                String.IsNullOrWhiteSpace(Price.ToString()) ||
                String.IsNullOrWhiteSpace(Summary) ||
                String.IsNullOrWhiteSpace(Genre.ToString()))
            {
                throw new ArgumentNullException("Одно или несколько полей не заданы!");
            }
        }
        public override string ToString()
        {
            return $"Комикс | ISBN: {Isbn} | Название: {Name} | Дата создания: {DateOfPrint} | Издательство: {Edition} |" +
                $"Количество в магазине: {Quantity} | Цена: {Price:C} | {Summary} | Жанр: {Genre}";
        }
    }
}
