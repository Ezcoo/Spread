using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spread;

public class BookService
{
    private readonly ApplicationDbContext _dbContext;
    public BookService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Book>> GetBooks()
    {
        var books = await _dbContext.Book.Include(b => b.Country).ToListAsync();

        return books;
    }

    public async Task<Book> AddBook(Book book)
    {
        _dbContext.Book.Add(book);
        await _dbContext.SaveChangesAsync();

        return book;
    }

    public async Task<Country?> GetCountry(string countryName)
    {
        var country = await _dbContext.Country.FirstOrDefaultAsync(c => c.Name == countryName);

        return country;
    }
}
