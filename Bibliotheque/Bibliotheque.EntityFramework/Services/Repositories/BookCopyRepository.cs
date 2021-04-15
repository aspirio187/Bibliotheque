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
        Task<bool> BookCopyExistAsync(int bookCopyId);
        Task<IEnumerable<BookCopyEntity>> GetBookCopiesAsync();
        Task<IEnumerable<BookCopyEntity>> GetBookCopiesAsync(int bookId);
        Task<IEnumerable<BookCopyEntity>> GetAvalaibleBookCopies(int bookId);
        Task<BookCopyEntity> GetBookCopyAsync(int bookCopyId);
        void AddBookCopy(BookCopyEntity bookCopy);
        void DeleteBookCopy(BookCopyEntity bookCopy);
        void DeleteBookCopies(IEnumerable<BookCopyEntity> bookCopies);
    }

    public partial class LibraryRepository : ILibraryRepository
    {
        public async Task<bool> BookCopyExistAsync(int bookCopyId)
        {
            return await m_Context.BookCopies.AnyAsync(x => x.Id == bookCopyId);
        }

        public async Task<IEnumerable<BookCopyEntity>> GetBookCopiesAsync()
        {
            return await m_Context.BookCopies.ToListAsync();
        }

        public async Task<IEnumerable<BookCopyEntity>> GetBookCopiesAsync(int bookId)
        {
            return await m_Context.BookCopies.Where(x => x.BookId == bookId).ToListAsync();
        }

        public async Task<IEnumerable<BookCopyEntity>> GetAvalaibleBookCopies(int bookId)
        {
            var copies = await m_Context.BookCopies.Where(c => c.BookId == bookId).ToListAsync();
            var borrows = m_Context.Borrows.AsEnumerable().Where(b => copies.Any(c => c.Id == b.BookCopyId)).ToList();
            foreach (var borrow in borrows)
            {
                var copy = copies.FirstOrDefault(c => c.Id == borrow.BookCopyId);
                if (copy is not null)
                {
                    if (copy.Quantity > 0) copy.Quantity--;
                }
            }
            return copies;
        }

        public async Task<BookCopyEntity> GetBookCopyAsync(int bookCopyId)
        {
            return await m_Context.BookCopies.FirstOrDefaultAsync(x => x.Id == bookCopyId);
        }

        public void AddBookCopy(BookCopyEntity bookCopy)
        {
            if (bookCopy is null) throw new ArgumentNullException(nameof(bookCopy));
            m_Context.BookCopies.Add(bookCopy);
        }

        public void DeleteBookCopy(BookCopyEntity bookCopy)
        {
            if (bookCopy is null) throw new ArgumentNullException(nameof(bookCopy));
            m_Context.BookCopies.Remove(bookCopy);
        }

        public void DeleteBookCopies(IEnumerable<BookCopyEntity> bookCopies)
        {
            m_Context.BookCopies.RemoveRange(bookCopies);
        }
    }
}
