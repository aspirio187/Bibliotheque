using Bibliotheque.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Services.Repositories
{
    public partial interface ILibraryRepository
    {
        Task<bool> BookExistsAsync(int bookId);
        IEnumerable<BookEntity> GetLastBooks();
        Task<IEnumerable<BookEntity>> GetBooksAsync();
        Task<BookEntity> GetBookAsync(int bookId);
        void AddBook(BookEntity book);
        void DeleteBook(BookEntity book);
    }

    public partial class LibraryRepository : ILibraryRepository
    {
        public async Task<bool> BookExistsAsync(int bookId)
        {
            return await m_Context.Books.AnyAsync(x => x.Id == bookId);
        }

        public IEnumerable<BookEntity> GetLastBooks()
        {
            var books = (from b in m_Context.Books
                         orderby b.ReleaseDate ascending
                         select b).Take(5);
            return books.ToList();
        }

        public async Task<IEnumerable<BookEntity>> GetBooksAsync()
        {
            return await m_Context.Books.ToListAsync();
        }

        public async Task<BookEntity> GetBookAsync(int bookId)
        {
            return await m_Context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
        }

        public void AddBook(BookEntity book)
        {
            if (book is null) throw new ArgumentNullException(nameof(book));
            m_Context.Entry(book).State = EntityState.Added;
        }

        public void DeleteBook(BookEntity book)
        {
            if (book is null) throw new ArgumentNullException(nameof(book));
            m_Context.Entry(book).State = EntityState.Deleted;
        }
    }
}
