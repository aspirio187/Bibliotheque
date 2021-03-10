using Bibliotheque.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.DbContexts
{
    public class LibraryContext : DbContext
    {
        #region Tous les DbSet
        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<BlackListedEntity> BlackListeds { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<BorrowEntity> Borrows { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ClassificationEntity> Classifications { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        #endregion

        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {

        }
    }
}
