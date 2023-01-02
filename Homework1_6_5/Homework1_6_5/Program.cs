using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Homework1_6_5
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CommandCaseAddBook = "1";
            const string CommandCaseRemoveBook = "2";
            const string CommandCaseShowAllBooks = "3";
            const string CommandCaseShowBooksByParameter = "4";
            const string CommandCaseExit = "5";

            bool isWork = true;
            BookStorage bookStorage = new BookStorage();

            while (isWork)
            {
                Console.Clear();
                Console.WriteLine(CommandCaseAddBook + ". Добавить книгу");
                Console.WriteLine(CommandCaseRemoveBook + ". Убрать кингу");
                Console.WriteLine(CommandCaseShowAllBooks + ". Показать все книги");
                Console.WriteLine(CommandCaseShowBooksByParameter + ". Показать книги по указанному параметру");
                Console.WriteLine(CommandCaseExit + ". Выход");

                Console.Write("Введите команду: ");
                string command = Console.ReadLine();

                switch (command)
                {
                    case CommandCaseAddBook:
                        bookStorage.AddBook();
                        break;

                    case CommandCaseRemoveBook:
                        bookStorage.RemoveBook();
                        break;

                    case CommandCaseShowAllBooks:
                        bookStorage.ShowAllBooks();
                        break;

                    case CommandCaseShowBooksByParameter:
                        bookStorage.ShowBooksByParameter();
                        break;

                    case CommandCaseExit:
                        isWork = false;
                        break;

                    default:
                        Console.WriteLine("Введена неверная команда");
                        break;
                }

                Console.ReadKey();
            }
        }
    }

    class Book
    {
        public string Name { get; private set; }
        public string Author { get; private set; }
        public int Year { get; private set; }

        public Book(string name, string author, int year)
        {
            Name = name;
            Author = author;
            Year = year;
        }
    }

    class BookStorage
    {
        private List<Book> _books = new List<Book>();

        public void AddBook()
        {
            Console.Write("Введите название книги: ");
            string name = Console.ReadLine();
            Console.Write("Введите автора книги: ");
            string author = Console.ReadLine();

            if (TryGetDate(out int year))
            {
                _books.Add(new Book(name, author, year));
                Console.WriteLine("Книга успешно добавлена");
            }
        }

        public void RemoveBook()
        {
            if (TryGetBook(out Book removedBook))
            {
                Console.WriteLine("Книга " + removedBook.Name + " удалена");
                _books.Remove(removedBook);
            }
        }

        public void ShowAllBooks()
        {
            int booksCounter = 1;

            foreach (var book in _books)
            {
                Console.WriteLine("\nКнига " + booksCounter + ":\n");
                Console.WriteLine("Название книги: " + book.Name);
                Console.WriteLine("Автор: " + book.Author);
                Console.WriteLine("Год издания: " + book.Year);

                booksCounter++;
            }
        }

        private bool TryGetBook(out Book book)
        {
            book = null;
            List<Book> booksFitsInputName = SearchBooksByName(_books);

            if (booksFitsInputName.Count==1)
            {
                book = booksFitsInputName[0];
                return true;
            }
            else if (booksFitsInputName.Count == 0)
            {
                Console.WriteLine("Такого названия нет");
                return false;
            }
            else
            {
                Console.Write("Есть несколько книг с таким названием.");
                List<Book> booksFitsInputAuthor = SearchBooksByAuthor(booksFitsInputName);

                if (booksFitsInputAuthor.Count == 1)
                {
                    book = booksFitsInputAuthor[0];
                    return true;
                }
                else if (booksFitsInputAuthor.Count == 0)
                {
                    Console.WriteLine("Такого автора нет");
                    return false;
                }
                else
                {
                    Console.Write("Есть несколько книг с таким автором.");

                    if (TrySearchBooksByYear(booksFitsInputAuthor, out List<Book> booksFitsInputYear))
                    {
                        book = booksFitsInputYear[0];
                        return true;
                    }

                    return false;
                }
            }
        }

        public void ShowBooksByParameter()
        {
            const string CommandCaseSearchByName = "1";
            const string CommandCaseSearchByAuthor = "2";
            const string CommandCaseSearchByYear = "3";

            Console.Clear();
            Console.WriteLine(CommandCaseSearchByName + ". Поиск по названию");
            Console.WriteLine(CommandCaseSearchByAuthor + ". Поиск по автору");
            Console.WriteLine(CommandCaseSearchByYear + ". Поиск по дате");

            Console.Write("\nВведите по какому признаку хотите устроить поиск: ");
            string command = Console.ReadLine();

            switch (command)
            {
                case CommandCaseSearchByName:
                    PrintBooksByName();
                    break;

                case CommandCaseSearchByAuthor:
                    PrintBooksByAuthor();
                    break;

                case CommandCaseSearchByYear:
                    PrintBooksByYear();
                    break;

                default:
                    Console.WriteLine("Неверно выбран признак");
                    break;
            }
        }

        private List<Book> SearchBooksByName(List <Book> books)
        {
            List<Book> booksFitsInputName = new List<Book>();
            Console.Write("Введите название книги: ");
            string nameInput = Console.ReadLine();

            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Name == nameInput)
                {
                    booksFitsInputName.Add(books[i]);
                }
            }

            return booksFitsInputName;
        }

        private List<Book> SearchBooksByAuthor(List<Book> books)
        {
            List<Book> booksFitsInputAuthor = new List<Book>();
            Console.Write("Введите автора: ");
            string authorInput = Console.ReadLine();

            for (int i = 0; i < books.Count; i++)
            {
                if (books[i].Author == authorInput)
                {
                    booksFitsInputAuthor.Add(books[i]);
                }
            }

            return booksFitsInputAuthor;
        }

        private bool TrySearchBooksByYear(List<Book> books, out List<Book> booksFitsInputYear)
        {
            booksFitsInputYear = new List<Book>();
            bool isSuccessSearch = true;

            if (TryGetDate(out int yearInput))
            {
                for (int i = 0; i < books.Count; i++)
                {
                    if (books[i].Year == yearInput)
                    {
                        booksFitsInputYear.Add(books[i]);
                    }
                }
            }

            if (booksFitsInputYear.Count == 0)
            {
                isSuccessSearch = false;
            }

            return isSuccessSearch;
        }

        private void PrintBooksByName()
        {
            List<Book> booksFitsInputName = SearchBooksByName(_books);

            Console.Clear();

            for (int i=0;i<booksFitsInputName.Count;i++)
            {
                Console.WriteLine(i+1 + ":");
                Console.WriteLine("\nНазвание: " + booksFitsInputName[i].Name);
                Console.WriteLine("Автор: " + booksFitsInputName[i].Author);
                Console.WriteLine("Год издания: " + booksFitsInputName[i].Year);
            }
        }

        private void PrintBooksByAuthor()
        {
            List<Book> booksFitsInputAuthor = SearchBooksByAuthor(_books);

            Console.Clear();

            for (int i = 0; i < booksFitsInputAuthor.Count; i++)
            {
                Console.WriteLine(i + 1 + ":");
                Console.WriteLine("\nНазвание: " + booksFitsInputAuthor[i].Name);
                Console.WriteLine("Автор: " + booksFitsInputAuthor[i].Author);
                Console.WriteLine("Год издания: " + booksFitsInputAuthor[i].Year);
            }
        }

        private void PrintBooksByYear()
        {
            if (TrySearchBooksByYear(_books, out List<Book> booksFitsInputYear))
            {
                Console.Clear();

                for (int i = 0; i < booksFitsInputYear.Count; i++)
                {
                    Console.WriteLine(i + 1 + ":");
                    Console.WriteLine("\nНазвание: " + booksFitsInputYear[i].Name);
                    Console.WriteLine("Автор: " + booksFitsInputYear[i].Author);
                    Console.WriteLine("Год издания: " + booksFitsInputYear[i].Year);
                }
            }
        }

        private bool TryGetDate(out int year)
        {
            Console.Write("Введите год выпуска книги: ");

            if (int.TryParse(Console.ReadLine(), out year))
            {
                if (year <= Convert.ToInt32(DateTime.Now.Year))
                {
                    return true;
                }
            }

            year = 0;
            Console.WriteLine("Ошибка ввода даты");
            return false;
        }
    }
}
