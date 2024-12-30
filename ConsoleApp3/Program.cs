namespace YourNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            Library library = new Library();
            Core core = new Core();
            bool isRunning = true;

            while (isRunning)
            {
                core.PrintMenu();
                int choice = core.GetIntInput("\n\nВыберите действие: ");

                switch (choice)
                {
                    case 1: // Добавить шкаф
                        Console.Clear();
                        core.AddCloset(library);
                        break;
                    case 2: // Удалить шкаф
                        Console.Clear();
                        core.RemoveCloset(library);
                        break;
                    case 3: // Добавить книгу
                        Console.Clear();
                        core.AddBook(library);
                        break;
                    case 4: // Удалить книгу
                        Console.Clear();
                        core.RemoveBook(library);
                        break;
                    case 5: // Найти книгу
                        Console.Clear();
                        core.FindBook(library);
                        break;
                    case 6: // Закрыть программу
                        Console.Clear();
                        isRunning = false;
                        break;
                    case 7: // Закрыть программу
                        isRunning = false;
                        break;
                }
            }
        }
    }

    class Core
    {
        public void PrintMenu()
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
            Console.WriteLine("\n6) Изменения книги");
            Console.WriteLine("\n7) Закрыть пргограмму");
        }

        public void AddCloset(Library library)
        {
            int shelfCount = GetIntInput("Введите количество полок: ");
            string description = GetStringInput("Введите описание шкафа: ");
            library.AddCloset(shelfCount, description);
            Console.Write("Шкаф добавлен!");
            Console.ReadKey();
        }

        public void RemoveCloset(Library library)
        {
            library.DisplayClosets();

            int closetNumber = GetIntInput("\nВведите номер шкафа для удаления: ");
            if (library.RemoveCloset(closetNumber))
            {
                Console.Write("Шкаф удалён");
            }
            else
            {
                Console.Write("Ошибка: шкаф не найден");
            }
            Console.ReadKey();
        }

        public void AddBook(Library library)
        {
            string title = GetStringInput("Введите название книги: ");
            string author = GetStringInput("Введите автора: ");
            string genre = GetStringInput("Введите жанр: ");
            string description = GetStringInput("Введите описание книги: ");
            Console.WriteLine();

            library.DisplayClosets();
            int closetNumber = GetIntInput("Введите номер шкафа: ");
            Console.WriteLine();
            library.DisplayShelves(closetNumber);
            int shelfNumber = GetIntInput("Введите номер полки: ");
            Console.WriteLine();

            if (library.AddBookToShelf(closetNumber, shelfNumber, title, author, genre, description))
            {
                Console.Write("Книга добавлена!");
            }
            else
            {
                Console.Write("Ошибка: не удалось добавить книгу");
            }
            Console.ReadKey();
        }

        public void RemoveBook(Library library)
        {
            string title = GetStringInput("Введите название книги для удаления: ");
            if (library.RemoveBookByTitle(title))
            {
                Console.Write("Книга успешно удалена.");
            }
            else
            {
                Console.Write("Ошибка: книга не найдена");
            }
            Console.ReadKey();
        }

        public void FindBook(Library library)
        {
            string title = GetStringInput("Введите название книги для поиска: ");
            Console.WriteLine();
            var (book, closetNumber, shelfNumber) = library.FindBookByTitle(title);

            if (book != null)
            {
                Console.WriteLine("Книга найдена:");
                Console.WriteLine($"Шкаф: {closetNumber}");
                Console.WriteLine($"Полка: {shelfNumber}");
                Console.WriteLine($"Название: {book.Title}");
                Console.WriteLine($"Автор: {book.Author}");
                Console.WriteLine($"Жанр: {book.Genre}");
                Console.WriteLine($"Описание: {book.Description}");
            }
            else
            {
                Console.Write("Книга не найдена");
            }
            Console.ReadKey();
        }


        public int GetIntInput(string prompt)
        {
            Console.Write(prompt);
            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Ошибка: введите корректное число: ");
            }
            return value;
        }

        public string GetStringInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine() ?? string.Empty;
        }
    }

    class Library
    {
        private List<Closet> _closets = new();

        public void AddCloset(int shelvesCount, string description)
        {
            _closets.Add(new Closet(shelvesCount, description));
        }

        public bool RemoveCloset(int closetNumber)
        {
            if (closetNumber >= 1 && closetNumber <= _closets.Count)
            {
                _closets.RemoveAt(closetNumber - 1);
                return true;
            }
            return false;
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
            {
                _closets[closetNumber - 1].DisplayShelves();
            }
            else
            {
                Console.Write("Ошибка: шкаф не найден");
            }
        }

        public bool AddBookToShelf(int closetNumber, int shelfNumber, string title, string author, string genre, string description)
        {
            if (closetNumber >= 1 && closetNumber <= _closets.Count)
            {
                return _closets[closetNumber - 1].AddBookToShelf(shelfNumber, title, author, genre, description);
            }
            return false;
        }

        //public Book? FindBookByTitle(string title)
        //{
        //    foreach (var closet in _closets)
        //    {
        //        var book = closet.FindBookByTitle(title);
        //        if (book != null)
        //        {
        //            return book;
        //        }
        //    }
        //    return null;
        //}
        public (Book? book, int closetNumber, int shelfNumber) FindBookByTitle(string title)
        {
            for (int i = 0; i < _closets.Count; i++)
            {
                var (book, shelfNumber) = _closets[i].FindBookByTitle(title);
                if (book != null)
                {
                    return (book, i + 1, shelfNumber); // Возвращаем книгу, номер шкафа и полки
                }
            }
            return (null, -1, -1); // Если книга не найдена, возвращаем null и -1
        }


        public bool RemoveBookByTitle(string title)
        {
            foreach (var closet in _closets)
            {
                if (closet.RemoveBookByTitle(title))
                {
                    return true;
                }
            }
            return false;
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

        //public Book? FindBookByTitle(string title)
        //{
        //    foreach (var shelf in _shelves)
        //    {
        //        var book = shelf.FindBookByTitle(title);
        //        if (book != null)
        //        {
        //            return book;
        //        }
        //    }
        //    return null;
        //}
        public (Book? book, int shelfNumber) FindBookByTitle(string title)
        {
            for (int i = 0; i < _shelves.Count; i++)
            {
                var book = _shelves[i].FindBookByTitle(title);
                if (book != null)
                {
                    return (book, i + 1); // Возвращаем книгу и номер полки
                }
            }
            return (null, -1); // Если книга не найдена, возвращаем null и -1
        }

        public bool RemoveBookByTitle(string title)
        {
            foreach (var shelf in _shelves)
            {
                if (shelf.RemoveBookByTitle(title))
                {
                    return true;
                }
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