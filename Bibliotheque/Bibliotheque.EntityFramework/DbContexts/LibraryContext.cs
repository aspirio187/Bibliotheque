using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.EntityFramework.StaticData;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.DbContexts
{
    public class LibraryContext : DbContext
    {
        #region Tous les DbSet
        public DbSet<AddressEntity> Addresses { get; set; }
        public DbSet<BlackListedEntity> BlackListeds { get; set; }
        public DbSet<BookCopyEntity> BookCopies { get; set; }
        public DbSet<BookEntity> Books { get; set; }
        public DbSet<BorrowEntity> Borrows { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<ClassificationEntity> Classifications { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=BibliothequeDb;Trusted_Connection=True;");
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Database=BibliothequeDb;Integrated Security=True;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (RolesEnum role in Enum.GetValues(typeof(RolesEnum)))
            {
                modelBuilder.Entity<RoleEntity>().HasData(new RoleEntity()
                {
                    Id = Guid.NewGuid(),
                    Name = RoleData.GetRole(role),
                    NormalizedName = RoleData.GetRole(role).ToUpper()
                });
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}
