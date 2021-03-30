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
        Task<bool> GenreExistsAsync(int genreId);
        Task<IEnumerable<GenreEntity>> GetGenresAsync();
        Task<IEnumerable<GenreEntity>> GetBookGenresAsync(int bookId);
        Task<GenreEntity> GetGenreAsync(int genreId);
        void AddGenre(GenreEntity genre);
        void DeleteGenre(GenreEntity genre);
    }

    public partial class LibraryRepository : ILibraryRepository
    {
        public async Task<bool> GenreExistsAsync(int genreId)
        {
            return await m_Context.Genres.AnyAsync(x => x.Id == genreId);
        }

        public async Task<IEnumerable<GenreEntity>> GetGenresAsync()
        {
            return await m_Context.Genres.ToListAsync();
        }

        public async Task<IEnumerable<GenreEntity>> GetBookGenresAsync(int bookId)
        {
            var genres = await m_Context.Genres.ToListAsync();
            return genres.Where(x => x.Books.Any(x => x.BookId == bookId));
        }

        public async Task<GenreEntity> GetGenreAsync(int genreId)
        {
            return await m_Context.Genres.FirstOrDefaultAsync(x => x.Id == genreId);
        }

        public void AddGenre(GenreEntity genre)
        {
            if (genre is null) throw new ArgumentNullException(nameof(genre));
            m_Context.Genres.Add(genre);
        }

        public void DeleteGenre(GenreEntity genre)
        {
            if (genre is null) throw new ArgumentNullException(nameof(genre));
            m_Context.Genres.Remove(genre);
        }
    }
}
