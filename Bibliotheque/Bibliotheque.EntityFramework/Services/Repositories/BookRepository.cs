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
        Task<IEnumerable<BookEntity>> GetBooks(string keyword);
        Task<IEnumerable<BookEntity>> GetBooksAsync();
        Task<BookEntity> GetBookAsync(int bookId);
        void AddBook(BookEntity book);
        void DeleteBook(BookEntity book);
        void DeleteBooks(IEnumerable<BookEntity> books);
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

        public async Task<IEnumerable<BookEntity>> GetBooks(string keyword)
        {
            var result = m_Context.Books as IQueryable<BookEntity>;
            string[] keywords = keyword.Split(' ');
            foreach (var word in keywords)
            {
                result = result.Where(x =>
                    x.Title.Contains(word) ||
                    x.Author.Contains(word) ||
                    x.Editor.Contains(word) ||
                    x.Format.Contains(word) ||
                    m_Context.Categories.Any(c => c.Id == x.CategoryId && c.Name.Contains(word)));
            }
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<BookEntity>> GetBooksAsync()
        {
            var books = await m_Context.Books.ToListAsync();
            foreach (var book in books)
            {
                await m_Context.Entry(book).Reference(x => x.Category).LoadAsync();
            }
            return books;
        }

        public async Task<BookEntity> GetBookAsync(int bookId)
        {
            return await m_Context.Books.FirstOrDefaultAsync(x => x.Id == bookId);
        }

        public void AddBook(BookEntity book)
        {
            m_Context.ChangeTracker.AutoDetectChangesEnabled = true;
            if (book is null) throw new ArgumentNullException(nameof(book));
            //m_Context.Books.Add(book);
            m_Context.Entry(book).State = EntityState.Added;
            if(book.CategoryId == 0)
            {
                m_Context.Entry(book.Category).State = EntityState.Added;
            }
            foreach (var genre in book.Genres)
            {
                if (m_Context.BooksGenres.Any(x => x.Id == genre.Id))
                {
                    var genreToUpdate = m_Context.BooksGenres.FirstOrDefault(x => x.Id == genre.Id);
                    genreToUpdate = genre;
                }
                else
                {
                    m_Context.Entry(genre).State = EntityState.Added;
                    if (genre.GenreId == 0)
                    {
                        m_Context.Entry(genre.Genre).State = EntityState.Added;
                    }
                    //m_Context.BooksGenres.Add(genre);
                }
            }
        }

        public void DeleteBook(BookEntity book)
        {
            if (book is null) throw new ArgumentNullException(nameof(book));
            m_Context.Entry(book).State = EntityState.Deleted;
        }

        public void DeleteBooks(IEnumerable<BookEntity> books)
        {
            m_Context.Books.RemoveRange(books);
        }
    }
}
