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
        public DbSet<GenreEntity> Classifications { get; set; }
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

            modelBuilder.Entity<CategoryEntity>().HasData(new CategoryEntity()
            {
                Id = 1,
                Name = "Roman"
            });

            modelBuilder.Entity<CategoryEntity>().HasData(new CategoryEntity()
            {
                Id = 2,
                Name = "Guide"
            });

            var b = new BookEntity()
            {
                Id = 1,
                Title = "Le thrône de fer - Tome 1: La glace et le feu",
                Author = "George R.R. Martin",
                Summary =
                $"En ces temps nimbés de brume, où la belle saison pouvait durer des années, la mauvaise, " +
                "toute une vie d'homme, se multiplièrent un jour des présages alarmants. Au nord du Mur " +
                "colossal qui protégeait le royaume des Sept Couronnes, se massèrent soudain des forces " +
                "obscures ; au sud, l'ordre établi chancela, la luxure, l'inceste, le meurtre, la corruption, " +
                "la lâcheté et le mensonge enserrèrent inexorablement le trône convoité. Dans la lignée des " +
                "Rois maudits, Le Trône de Fer plonge le lecteur dans un univers où l'épique et le chevaleresque " +
                "côtoient sans cesse la duplicité et la fourberie. Mais dans ce tourbillon d'aventures cruelles, " +
                "c'est finalement l'indestructible force de l'amitié qui rayonne au-dessus des ténèbres.",
                ReleaseDate = new DateTime(1999, 1, 14),
                Editor = "Pygmalion",
                Format = "Roman (broché)",
                Pages = 382,
                EAN = "9782857045465",
                ISBN = "2857045468",
                Preface = "../../../Images/le throne de fer t1.jpg",
                CategoryId = 1
            };

            b.Genres.Add(new GenreEntity()
            {
                Id = 1,
                Name = "Fantasy littéraire"
            });

            b.Genres.Add(new GenreEntity()
            {
                Id = 2,
                Name = "Fantasy épique"
            });
            modelBuilder.Entity<BookEntity>().HasData(b);

            b = new BookEntity()
            {
                Id = 2,
                Title = "20 milles lieues sous les mers",
                Author = "Jules Verne",
                Summary =
                $"La Marine américaine dépêche le professeur Aronnax pour débarrasser les océans du monstre marin " +
                "qui coule ses navires. Mais alors que la rencontre tant attendue se produit, le professeur est " +
                "loin de se douter qu'un fabuleux voyage sous-marin l'attend. Version abrégée de l'épopée du " +
                "Nautilus et du capitaine Nemo. ©Electre 2021",
                ReleaseDate = new DateTime(1976, 3, 1),
                Editor = "Lgf",
                Format = "Roman (Poche)",
                Pages = 672,
                EAN = "9782253006329",
                ISBN = "2253006327",
                Preface = "../../../Images/Vingt mille lieues sous les mers.jpg",
                CategoryId = 1
            };

            b.Genres.Add(new GenreEntity()
            {
                Id = 3,
                Name = "Roman d'aventures"
            });
            modelBuilder.Entity<BookEntity>().HasData(b);

            b = new BookEntity()
            {
                Id = 3,
                Title = "La Bourse pour les Nuls (5e édition)",
                Author = "Gérard Horny",
                Summary =
                $"La Bourse pour les nulsToutes les clés pour faire fructifier son patrimoine ! Quelles sont les " +
                "grandes places financières mondiales Comment passer un ordre en Bourse Quels sont les placements les " +
                "plus rentables Rédigé par un spécialiste de l'investissement boursier, cet ouvrage s'adresse à tous " +
                "ceux qui veulent comprendre facilement le fonctionnement de la Bourse.Que vous soyez un particulier " +
                "curieux, ou bien un épargnant soucieux de valoriser votre patrimoine, vous trouverez les réponses à " +
                "vos questions dans ce livre ! Découvrez. Les acteurs principaux des marchés. La bonne gestion de votre" +
                "portefeuille. Les outils d'aide à la décision. Les nouvelles formes d'investissement. Les règles " +
                "fiscales des valeurs mobilières",
                ReleaseDate = new DateTime(2020, 1, 23),
                Editor = "First",
                Format = "Guide (broché)",
                Pages = 500,
                EAN = "9782412048146",
                ISBN = "2412022631",
                Preface = "../../../Images/La bours pour les nuls.jpg",
                CategoryId = 2
            };

            b.Genres.Add(new GenreEntity()
            {
                Id = 4,
                Name = "Instruction",
            });

            b.Genres.Add(new GenreEntity()
            {
                Id = 5,
                Name = "Référence"
            });
            modelBuilder.Entity<BookEntity>().HasData(b);

            base.OnModelCreating(modelBuilder);
        }
    }
}
