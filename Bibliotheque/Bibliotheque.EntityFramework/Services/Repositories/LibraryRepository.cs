using Bibliotheque.EntityFramework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Services.Repositories
{
    public partial class LibraryRepository : ILibraryRepository
    {
        private readonly LibraryContext m_Context;

        public LibraryRepository(LibraryContext context)
        {
            m_Context = context ??
                throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Enregistre les modifications dans la base de donnée
        /// </summary>
        /// <returns>Le nombre de champs sur lesquelles une 
        /// action a été effectuée</returns>
        public async Task<int> SaveAsync()
        {
            return await m_Context.SaveChangesAsync();
        }
    }
}
