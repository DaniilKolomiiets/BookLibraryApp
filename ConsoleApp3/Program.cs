using System;

namespace YourNamespace // Замените на подходящее имя пространства имен
{
    class Program
    {
        static void Main(string[] args) // Точка входа в программу
        {
            bool isOpen = true;
            Library library = new Library();
            Prog prog = new Prog();

            while (isOpen)
            {
                
                prog.doMenu();


                int choice = prog.getIntInput("\n\nВыбор действия: ");


                switch (choice.ToString())
                {
                    case "1":
                        Console.Clear();

                        int shelfValue = prog.getIntInput("Введите количество полок: ");
                        Console.Clear();
                        string discription = prog.getStringInput("Введите описание шкафа (где он, какой это шкаф и любая полезная для вас информация: ");
                        Console.Clear();
                        library.creatCloset(Convert.ToInt32(shelfValue), discription);
                        Console.Write($"Шкаф номер {library.getClosetValue()} создан\n\nНажмите любую клавишу, чтоб продолжить...");
                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();

                        for (int i = 0; i < library.getClosetValue(); i++)
                        {
                            Console.WriteLine($"Шкаф номер {i + 1}");
                        }


                        int closetNumber = prog.getIntInput("\n\n\nКакой шкаф вы хотите удалить?\n\nНомер шкафа: ");
                        library.deleteCloset(closetNumber);
                        Console.Clear();
                        Console.WriteLine($"Шкаф {closetNumber} удален.\n\nНажмите любую клавишу, чтоб продолжить...");

                        Console.ReadKey(true);
                        Console.Clear();
                        break;
                    case "3":
                        Console.Clear();
                        string tittle = prog.getStringInput("Введите Название книги: ");
                        Console.Clear();


                        string author = prog.getStringInput("Введите автора: ");
                        Console.Clear();


                        string genre = prog.getStringInput("Введите жанр: ");
                        Console.Clear();

                        string discriptio = prog.getStringInput("Введите описание книги: ");
                        Console.Clear();
                        for (int i = 0; i < library.getClosetValue(); i++)
                        {
                            Console.WriteLine($"Шкаф номер {i + 1}");
                        }


                        int closetNum = prog.getIntInput("\n\nВведите номер шкафа: ");

                        Console.Clear();
                        for (int i = 0; i < library.getShelfValue(closetNum); i++)
                        {
                            Console.WriteLine($"Полка номер {i + 1}");
                        }


                        int shelfNum = prog.getIntInput("\n\nВведите номер Полки: ");
                        Console.Clear();
                        library.createBook(closetNum, shelfNum, tittle, author, discriptio, genre);
                        Console.WriteLine("Книга успешно добавлена!\n\nНажмите любую клавишу, чтоб продолжить...");
                        Console.ReadKey(true);
                        Console.Clear();

                        break;
                    case "4":
                        Console.Clear();
                        Console.WriteLine("1) Найти книгу вручную\nНайти книгу по названию");
                        int choi = prog.getIntInput("\n\nВыбор действия: ");
                        if (choi == 1)
                        {

                        }
                        else if (choi == 2)
                        {

                        }
                        else { }
                        break;
                    case "5":
                        break;
                    case "6":
                        break;
                    case "7":
                        break;
                    case "8":
                        isOpen = false;
                        break;
                }
            }
        }
    }
}


class Prog
{
    public void doMenu()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("=====Меню=====");
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("\n1) Добавить Шкаф");
        Console.WriteLine("\n2) Удалить шкаф");
        Console.WriteLine("\n3) Добавить книгу");
        Console.WriteLine("\n4) Удалить книгу");
        Console.WriteLine("\n5) Найти книгу");
        Console.WriteLine("\n6) Изменения книги");
        Console.WriteLine("\n7) Помощь");
        Console.WriteLine("\n8) Закрыть пргограмму");
    }

    public int getIntInput(string prompt)
    {
        Console.Write(prompt);
        int result; 

        while (!int.TryParse(Console.ReadLine(), out result))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ошибка: введите корректное число.");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("\n\nНажмите любую клавишу, чтоб продолжить..");
            Console.ReadKey(true);
            
            Console.Write(prompt);
        }

        return result; 
    }

    public string getStringInput(string prompt)
    {
        Console.Write(prompt);
        return Console.ReadLine() ?? string.Empty;
    }
}

class Book
{
    private string _tittle;
    private string _author;
    private string _description;
    private string _genre;

    public Book(string tittle, string author, string description, string genre)
    {
        _tittle = tittle;
        _author = author;
        _description = description;
        _genre = genre;
    }
}

class Closet
{
    private int _shelfsValue;
    private string _closetDiscription;
    private List<Shelf> _shelfs;

    public Closet(int shelfsValue, string closetDiscription)
    {
        _shelfs = new List<Shelf>();
        _shelfsValue = shelfsValue;

        createShelfs();
        _closetDiscription = closetDiscription;
    }

    private void createShelfs()
    {
        for (int i = 0; i < _shelfsValue; i++)
        {
            _shelfs.Add(new Shelf());
        }
    }

    public int getShelfValue() { return _shelfsValue; }

    public void createBook(int ShelfNum, string tittle, string author, string description, string genre)
    {
        _shelfs[ShelfNum].addBook(tittle, author, description, genre);
    }
}

class Shelf
{
    private List<Book> _books;

    public Shelf()
    {
        _books = new List<Book>();
    }

    public void addBook(string tittle, string author, string description, string genre)
    {
        _books.Add(new Book(tittle, author, description, genre));
    }
}

class Library
{
    private List<Closet> _closets;

    public Library()
    {
        _closets = new List<Closet>();
    }

    public void creatCloset(int shelfsValue, string closetDiscription)
    {
        _closets.Add(new Closet(shelfsValue, closetDiscription));
    }

    public int getClosetValue() { return _closets.Count; }

    public void deleteCloset(int closetNumber)
    {
        _closets.RemoveAt(closetNumber - 1);
    }

    public int getShelfValue(int numberOfCloset) { return _closets[numberOfCloset].getShelfValue(); }

    public void createBook(int closetNum, int ShelfNum, string tittle, string author, string description, string genre)
    {
        _closets[closetNum].createBook(ShelfNum, tittle, author, description, genre);
    }
}