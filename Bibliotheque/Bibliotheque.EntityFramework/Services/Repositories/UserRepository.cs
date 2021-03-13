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
        Task<bool> UserExistsAsync(Guid userId);
        Task<bool> EmailExistsAsync(string email);
        void AddUser(UserEntity user);
        Task<IEnumerable<UserEntity>> GetUsersAsync();
        Task<UserEntity> GetUserAsync(Guid userId);
        void DeleteUser(UserEntity user);
    }

    public partial class LibraryRepository : ILibraryRepository
    {
        /// <summary>
        /// Vérifie s'il existe un utilisateur avec l'id en paramètre
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <returns>true Si l'utilisateur existe. false Dans le cas contraire</returns>
        public async Task<bool> UserExistsAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));
            return await m_Context.Users.AnyAsync(x => x.Id == userId);
        }

        /// <summary>
        /// Vérifie si le mail en paramètre existe déjà dans la base de données
        /// </summary>
        /// <param name="email">Chaine de caractère qui représente le mail</param>
        /// <returns>true Si le mail existe. false Dans le cas contraire</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));
            return await m_Context.Users.AnyAsync(x => x.Email == email);
        }

        /// <summary>
        /// Ajoute un utilisateur dans le contexte
        /// </summary>
        /// <param name="user">Entité User à ajouter</param>
        public void AddUser(UserEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            m_Context.Entry(user).State = EntityState.Added;
        }

        /// <summary>
        /// Retourne la liste de tous les utilisateurs dans le contexte
        /// </summary>
        /// <returns>IEnumerable des entités User</returns>
        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            return await m_Context.Users.ToListAsync();
        }

        /// <summary>
        /// Retourne l'utilisateur choisi par son ID dans le contexte
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <returns>Une entité User</returns>
        public async Task<UserEntity> GetUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));
            return await m_Context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        /// <summary>
        /// Supprime l'entité User en paramètre du contexte
        /// </summary>
        /// <param name="user">Entité User</param>
        public void DeleteUser(UserEntity user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            m_Context.Users.Remove(user);
        }
    }
}
