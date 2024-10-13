using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;

namespace Spread;
internal class Program
{
    static void Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
        {
            services.AddTransient<SpreadApplication>();
            services.AddTransient<BookService>();
            services.AddDbContext<ApplicationDbContext>();
        });

        var hostBuilder = host.Build();

        var application = hostBuilder.Services.GetRequiredService<SpreadApplication>();

        Task.Run(async () =>
        {
            await application.Run();
        }).Wait();

    }
}

public class SpreadApplication
{
    private readonly BookService _bookService;
    public SpreadApplication(BookService bookService)
    {
        Console.WriteLine($"Booting started! {nameof(SpreadApplication)}");

        _bookService = bookService;

    }

    public async Task Run()
    {
        bool isRunning = true;

        while (isRunning)
        {
            Console.Write("Type a command: ");
            string input = Console.ReadLine();
            input = input.ToLower();
            
            switch (input)
            {
                case null:
                    break;

                case "get-books":
                    await GetBooks();
                    break;

                case "add-book":
                    await AddBook();
                    break;

                case "add-country":
                    await AddCountry();
                    break;

                case "quit":
                    isRunning = false;
                    break;

                default:
                    Console.WriteLine("Unknown command!");
                    break;
            }
        }
    }

    private async Task GetBooks()
    {
        List<Book> books = await _bookService.GetBooks();

        Console.WriteLine();

        foreach (Book book in books)
        {
            Console.WriteLine(book.Title + " (" + book.Country.Name + ")");
        }

        Console.WriteLine();
    }

    private async Task AddBook()
    {
        Console.Write("Please type the name of the book: ");
        string bookName = Console.ReadLine();

        while (bookName == null)
        {
            Console.Write("Invalid book name. Please try again: ");
            bookName = Console.ReadLine();
        }

        Console.Write("Please type the country of the book: ");
        var countryName = Console.ReadLine();

        while (countryName == null)
        {
            Console.Write("Invalid country name. Please try again: ");
            countryName = Console.ReadLine();
        }

        var country = await _bookService.GetCountry(countryName);

        while (country == null)
        {
            Console.Write("Country not found from database! If you would like to add a country to the database, use command \'add-country\'. In other case, please try again: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case null:
                    break;

                case "add-country":
                    await AddCountry();
                    country = await _bookService.GetCountry(countryName);
                    break;

                default:
                    country = await _bookService.GetCountry(countryName);
                    break;
            }

        }

        Book book = await _bookService.AddBook(new Book { Title = bookName, Country = country });
        
        Console.WriteLine($"Book \'{bookName}\' ({country.Name}) added to database!\n");
    }

    private async Task AddCountry()
    {
        Console.Write("Please type the name of the country: ");
        string countryName = Console.ReadLine();

        while (countryName == null)
        {
            Console.Write("Invalid country name. Please try again: ");
            countryName = Console.ReadLine();
        }

        var country = await _bookService.AddCountry(countryName);
        Console.WriteLine($"Country \'{country.Name}\' added to database!\n");
    }

}