using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibraryEME
{
    public class ItemAlreadyExistException : Exception
    {
        public ItemAlreadyExistException() { }
        public ItemAlreadyExistException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException() { }
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class IllegalIsbnException : Exception
    {
        public IllegalIsbnException() { }
        public IllegalIsbnException(string message) : base(message) { }
    }

    public class ItemOutOfStockExceptions : Exception
    {
        public ItemOutOfStockExceptions() { }
        public ItemOutOfStockExceptions(string message) : base(message) { }
    }

    public class IllegalIdException : Exception
    {
        public IllegalIdException() { }
        public IllegalIdException(string message) : base(message) { }
    }
}
