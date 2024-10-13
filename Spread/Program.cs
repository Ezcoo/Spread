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
            string input = Console.ReadLine();
            if (input == null)
            {
                continue;
            }
            input = input.ToLower();

            if (input == "get-books")
            {
                await GetBooks();
            }
            else if (input == "add-book")
            {
                await AddBook();
            }
            else if (input == "quit")
            {
                isRunning = false;
            }
            else
            {
                Console.WriteLine("Unknown command!");
            }
        }
    }

    private async Task GetBooks()
    {
        List<Book> books = await _bookService.GetBooks();

        foreach (Book book in books)
        {
            Console.WriteLine(book.Title + ", " + book.Country.Name);
        }
    }

    private async Task AddBook()
    {
        Console.Write("Please type the name of the book: ");
        // TODO: Handle null / empty book name
        string bookName = Console.ReadLine();

        // TODO: Handle no country specified
        Console.Write("Please type the country of the book: ");
        var country = await _bookService.GetCountry(Console.ReadLine());

        if (country != null)
        {
           Book book = await _bookService.AddBook(new Book { Title = bookName, Country = country });
        }
    }
}
