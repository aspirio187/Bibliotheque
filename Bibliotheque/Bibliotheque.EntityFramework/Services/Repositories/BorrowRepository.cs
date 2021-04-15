using Bibliotheque.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Services.Repositories
{
    public partial interface ILibraryRepository
    {
        void AddBorrow(BorrowEntity borrowEntity);
        void DeleteBorrow(BorrowEntity borrowEntity);
    }

    public partial class LibraryRepository : ILibraryRepository
    {
        public void AddBorrow(BorrowEntity borrowEntity)
        {
            if (borrowEntity is null) throw new ArgumentNullException(nameof(borrowEntity));
            m_Context.Borrows.Add(borrowEntity);
        }

        public void DeleteBorrow(BorrowEntity borrowEntity)
        {
            if (borrowEntity is null) throw new ArgumentNullException(nameof(borrowEntity));
            m_Context.Borrows.Remove(borrowEntity);
        }
    }
}
