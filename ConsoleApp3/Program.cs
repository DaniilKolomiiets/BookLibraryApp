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
                int choice = Menu.GetIntInput("\n\nВыберите действие: ");
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
            Console.WriteLine("=====Меню=====");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("\n1) Добавить Шкаф");
            Console.WriteLine("\n2) Удалить шкаф");
            Console.WriteLine("\n3) Добавить книгу");
            Console.WriteLine("\n4) Удалить книгу");
            Console.WriteLine("\n5) Найти книгу");
            Console.WriteLine("\n6) Закрыть пргограмму");
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
                Console.Write("Ошибка: введите корректное число: ");
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
            int shelfCount = Menu.GetIntInput("Введите количество полок: ");
            string description = Menu.GetStringInput("Введите описание шкафа: ");
            _closets.Add(new Closet(shelfCount, description));
            Console.Write("Шкаф добавлен!");

            Console.ReadKey();
        }

        public void RemoveCloset()
        {
            DisplayClosets();
            int closetNumber = Menu.GetIntInput("\nВведите номер шкафа для удаления: ");

            if (closetNumber >= 1 && closetNumber <= _closets.Count)
            {
                _closets.RemoveAt(closetNumber - 1);
                Console.Write("Шкаф удалён");
            }
            else
            {
                Console.Write("Ошибка: шкаф не найден");
            }

            Console.ReadKey();
        }

        public void DisplayClosets()
        {
            for (int i = 0; i < _closets.Count; i++)
            {
                Console.WriteLine($"Шкаф {i + 1}: {_closets[i].Description}");
            }
        }

        public void DisplayShelves(int closetNumber)
        {
            if (closetNumber >= 1 && closetNumber <= _closets.Count)
                _closets[closetNumber - 1].DisplayShelves();
            else
                Console.Write("Ошибка: шкаф не найден");
        }

        public void AddBook()
        {
            string title = Menu.GetStringInput("Введите название книги: ");
            string author = Menu.GetStringInput("Введите автора: ");
            string genre = Menu.GetStringInput("Введите жанр: ");
            string description = Menu.GetStringInput("Введите описание книги: ");

            DisplayClosets();
            int closetNumber = Menu.GetIntInput("Введите номер шкафа: ");
            DisplayShelves(closetNumber);
            int shelfNumber = Menu.GetIntInput("Введите номер полки: ");

            if (closetNumber >= 1 && closetNumber <= _closets.Count &&
                _closets[closetNumber - 1].AddBookToShelf(shelfNumber, title, author, genre, description))
            {
                Console.Write("Книга добавлена!");
            }
            else
            {
                Console.Write("Ошибка: не удалось добавить книгу");
            }

            Console.ReadKey();
        }

        public void FindBook()
        {
            string title = Menu.GetStringInput("Введите название книги для поиска: ");

            for (int i = 0; i < _closets.Count; i++)
            {
                var (book, shelf) = _closets[i].FindBookByTitle(title);

                if (book != null)
                {
                    Console.WriteLine("Книга найдена:");
                    Console.WriteLine($"Шкаф: {i + 1}");
                    Console.WriteLine($"Полка: {shelf}");
                    Console.WriteLine($"Название: {book.Title}");
                    Console.WriteLine($"Автор: {book.Author}");
                    Console.WriteLine($"Жанр: {book.Genre}");
                    Console.WriteLine($"Описание: {book.Description}");
                    break;
                }
                else
                {
                    Console.Write("Книга не найдена");
                }
            }

            Console.ReadKey();
        }


        public void RemoveBook()
        {
            string title = Menu.GetStringInput("Введите название книги для удаления: ");

            foreach (var closet in _closets)
            {
                if (closet.RemoveBookByTitle(title))
                    Console.Write("Книга успешно удалена.");
                else
                    Console.Write("Ошибка: книга не найдена");
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
                Console.WriteLine($"Полка {i + 1}");
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

        public (Book? book, int shelfNumber) FindBookByTitle(string title)
        {
            for (int i = 0; i < _shelves.Count; i++)
            {
                var book = _shelves[i].FindBookByTitle(title);

                if (book != null)
                    return (book, i + 1); // Возвращаем книгу и номер полки

            }

            return (null, -1); // Если книга не найдена, возвращаем null и -1
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