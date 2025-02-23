using System.Data.SQLite;

namespace Kolomiiets.Library
{
    class Program
    {
        static void Main()
        {
            Library library = new();
            bool isRunning = true;

            while (isRunning)
            {
                Menu.PrintMenu();
                int choice = Menu.GetIntInput("\n\nSelect an action: ");
                Console.Clear();

                switch (choice)
                {
                    case 1:
                        library.AddCloset();
                        break;
                    case 2:
                        library.RemoveCloset();
                        break;
                    case 3:
                        library.AddBook();
                        break;
                    case 4:
                        library.RemoveBook();
                        break;
                    case 5:
                        library.FindBook();
                        break;
                    case 6:
                        isRunning = false;
                        break;
                }
            }
        }
    }

    static class Menu
    {
        public static void PrintMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=====Menu=====");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n1) Add a closet");
            Console.WriteLine("\n2) Remove the closet");
            Console.WriteLine("\n3) Add a book");
            Console.WriteLine("\n4) Remove the book");
            Console.WriteLine("\n5) Finde the book");
            Console.WriteLine("\n6) Close the App");
        }

        public static int GetIntInput(string prompt)
        {
            Console.Write(prompt);

            while (true)
            {
                string? input = Console.ReadLine();

                if (int.TryParse(input, out int value))
                {
                    Console.WriteLine();
                    return value;
                }
                Console.Write("Error: Enter the correct number: ");
            }
        }

        public static string GetStringInput(string prompt)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            Console.WriteLine();
            return input ?? string.Empty;
        }
    }

    class Library
    {
        private List<Closet> _closets = new();

        public void AddCloset()
        {
            int shelfCount = Menu.GetIntInput("Enter the number of shelves: ");
            string description = Menu.GetStringInput("Enter a description of the closet: ");
            _closets.Add(new Closet(shelfCount, description));
            Console.Write("Closet added!");

            Console.ReadKey();
        }

        public void RemoveCloset()
        {
            DisplayClosets();
            int closetNumber = Menu.GetIntInput("\nEnter the closet number to remove: ");

            if (closetNumber >= 1 && closetNumber <= _closets.Count)
            {
                _closets.RemoveAt(closetNumber - 1);
                Console.Write("Closet removed");
            }
            else
            {
                Console.Write("Error: closet not found");
            }

            Console.ReadKey();
        }

        public void DisplayClosets()
        {
            for (int i = 0; i < _closets.Count; i++)
            {
                Console.WriteLine($"Closet {i + 1}: {_closets[i].Description}");
            }
        }

        public void DisplayShelves(int closetNumber)
        {
            if (closetNumber >= 1 && closetNumber <= _closets.Count)
                _closets[closetNumber - 1].DisplayShelves();
            else
                Console.Write("Error: closet not found");
        }

        public void AddBook()
        {
            string title = Menu.GetStringInput("Enter the title of the book: ");
            string author = Menu.GetStringInput("Enter the author's name: ");
            string genre = Menu.GetStringInput("Enter the genre: ");
            string description = Menu.GetStringInput("Enter a description of the book: ");

            DisplayClosets();
            int closetNumber = Menu.GetIntInput("Enter the closet number: ");
            DisplayShelves(closetNumber);
            int shelfNumber = Menu.GetIntInput("Enter the shelf number: ");

            if (closetNumber >= 1 && closetNumber <= _closets.Count &&
                _closets[closetNumber - 1].AddBookToShelf(shelfNumber, title, author, genre, description))
            {
                Console.Write("Book added!");
            }
            else
            {
                Console.Write("Error: failed to add book");
            }

            Console.ReadKey();
        }

        public void FindBook()
        {
            string title = Menu.GetStringInput("Enter the title of the book to search for: ");

            for (int i = 0; i < _closets.Count; i++)
            {
                var (book, shelf) = _closets[i].FindBookByTitle(title);

                if (book != null)
                {
                    Console.WriteLine("Book found:");
                    Console.WriteLine($"Closet: {i + 1}");
                    Console.WriteLine($"Shelf: {shelf}");
                    Console.WriteLine($"Title: {book.Title}");
                    Console.WriteLine($"Author: {book.Author}");
                    Console.WriteLine($"Genre: {book.Genre}");
                    Console.WriteLine($"Description: {book.Description}");
                    break;
                }
                else
                {
                    Console.Write("Book not found");
                }
            }

            Console.ReadKey();
        }


        public void RemoveBook()
        {
            string title = Menu.GetStringInput("Enter the title of the book to be removed: ");

            foreach (var closet in _closets)
            {
                if (closet.RemoveBookByTitle(title))
                    Console.Write("The book has been successfully removed");
                else
                    Console.Write("Error: Book not found");
            }

            Console.ReadKey();
        }
    }

    class Closet
    {
        public string Description { get; }
        private List<Shelf> _shelves = new();

        public Closet(int shelvesCount, string description)
        {
            Description = description;

            for (int i = 0; i < shelvesCount; i++)
            {
                _shelves.Add(new Shelf());
            }
        }

        public void DisplayShelves()
        {
            for (int i = 0; i < _shelves.Count; i++)
            {
                Console.WriteLine($"Shelf {i + 1}");
            }
        }

        public bool AddBookToShelf(int shelfNumber, string title, string author, string genre, string description)
        {
            if (shelfNumber >= 1 && shelfNumber <= _shelves.Count)
            {
                _shelves[shelfNumber - 1].AddBook(new Book(title, author, genre, description));
                return true;
            }

            return false;
        }

        public (Book? book, int? shelfNumber) FindBookByTitle(string title)
        {
            for (int i = 0; i < _shelves.Count; i++)
            {
                var book = _shelves[i].FindBookByTitle(title);

                if (book != null)
                    return (book, i + 1);

            }

            return (null, null);
        }

        public bool RemoveBookByTitle(string title)
        {
            foreach (var shelf in _shelves)
            {
                if (shelf.RemoveBookByTitle(title))
                    return true;
            }

            return false;
        }
    }

    class Shelf
    {
        private List<Book> _books = new();

        public void AddBook(Book book) => _books.Add(book);

        public Book? FindBookByTitle(string title)
        {
            return _books.FirstOrDefault(b => b.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public bool RemoveBookByTitle(string title)
        {
            var book = FindBookByTitle(title);

            if (book != null)
            {
                _books.Remove(book);
                return true;
            }

            return false;
        }
    }

    class Book
    {
        public string Title { get; }
        public string Author { get; }
        public string Genre { get; }
        public string Description { get; }

        public Book(string title, string author, string genre, string description)
        {
            Title = title;
            Author = author;
            Genre = genre;
            Description = description;
        }
    }
}