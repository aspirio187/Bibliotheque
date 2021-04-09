using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.EntityFramework.Helpers;
using Bibliotheque.EntityFramework.StaticData;
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
        Task<bool> UserIsBlackListed(Guid userId);
        Task<bool> UserTokenHasChanged(Guid userId, Guid token);
        void AddUser(UserEntity user);
        Task<IEnumerable<UserEntity>> GetUsersAsync();
        Task<UserEntity> GetUserAsync(Guid userId);
        void DeleteUser(UserEntity user);
    }

    // TODO : Ajouter les tags d'exception dans les commentaires
    // TODO : Déplacer UserIsBlackListed() dans BlackListedRepository
    public partial class LibraryRepository : ILibraryRepository
    {
        /// <summary>
        /// Vérifie s'il existe dans le contexte un utilisateur ayant comme ID celui 
        /// donné en paramètre. Si l'id en paramètre est vide, le programme lance une
        /// exception.
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <returns>
        /// true Si l'utilisateur existe. false Dans le cas contraire
        /// </returns>
        public async Task<bool> UserExistsAsync(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException(nameof(userId));
            return await m_Context.Users.AnyAsync(x => x.Id == userId);
        }

        /// <summary>
        /// Vérifie s'il existe dans le contexte, une entité UserEntity ayant déjà
        /// l'adresse mail en paramètre
        /// </summary>
        /// <param name="email">
        /// Chaine de caractère qui représente l'adresse email recherchée
        /// </param>
        /// <returns>
        /// true Si le mail existe. false Dans le cas contraire
        /// </returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));
            if (email.Equals(string.Empty)) throw new ArgumentException(nameof(email));
            return await m_Context.Users.AnyAsync(x => x.Email == email);
        }

        /// <summary>
        /// Vérifie si l'utilisateur défini par l'ID en paramètre est dans la liste
        /// des utilisateurs blacklisté
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <returns>true Si l'utilisateur est blacklisté. false Dans le cas contraire</returns>
        public async Task<bool> UserIsBlackListed(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException(nameof(userId));
            return await m_Context.BlackListeds.AnyAsync(x => x.UserId == userId);
        }

        /// <summary>
        /// Vérifie si le token de l'utilisateur ayant comme ID celui donnée en paramètre
        /// est identique à celui donnée en paramètre. Si l'ID ou le token est vide, le 
        /// programme lance une exception
        /// </summary>
        /// <param name="userId">ID de l'utilisateur</param>
        /// <param name="token">Token a comparer</param>
        /// <returns>
        /// true Si le token est identique. false Dans le cas contraire
        /// </returns>
        public async Task<bool> UserTokenHasChanged(Guid userId, Guid token)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (token == Guid.Empty) throw new ArgumentNullException(nameof(token));
            var user = await m_Context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null) throw new ArgumentNullException(nameof(user));
            return !user.Token.Equals(token);
        }

        /// <summary>
        /// Ajoute un utilisateur dans le contexte. Si l'entité UserEntity ou sa propriété
        /// AddressEntity sont null ou si le role USER est introuvable, le programme lance 
        /// une exception
        /// </summary>
        /// <param name="user">Entité User à ajouter</param>
        public void AddUser(UserEntity user)
        {
            // Vérifie si l'entité utilisateur en paramètre est null
            if (user == null) throw new ArgumentNullException(nameof(user));
            // Récupère le role utilisateur pour le nouvel utilisateur
            user.Role = m_Context.Roles.FirstOrDefault(x => x.Name.Equals(RoleData.GetRole(RolesEnum.User)));
            // Vérifie si le role est null
            if (user.Role is null) throw new ArgumentNullException(nameof(user.Role));
            // Crée un ID à l'utilisateur
            user.Id = Guid.NewGuid();
            // Crée un nouveau Token pour l'utilisateur
            user.Token = Guid.NewGuid();
            //// Ajout de l'addresse à l'utilisateur
            //address.UserId = user.Id;
            // Ajoute l'utilisateur au contexte
            m_Context.Users.Add(user);
        }

        /// <summary>
        /// Retourne la liste de toutes les entités UserEntity dans le contexte
        /// </summary>
        /// <returns>
        /// IEnumerable des entités UserEntity
        /// </returns>
        public async Task<IEnumerable<UserEntity>> GetUsersAsync()
        {
            return await m_Context.Users.ToListAsync();
        }

        /// <summary>
        /// Retourne une entité UserEntity du contexte dont la propriété ID est égale à l'id 
        /// en paramètre
        /// </summary>
        /// <param name="userId">ID de l'entité recherchée</param>
        /// <returns>
        /// L'entité UserEntity recherchée
        /// </returns>
        public async Task<UserEntity> GetUserAsync(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException(nameof(userId));
            return await m_Context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        /// <summary>
        /// Supprime l'entité UserEntity du contexte
        /// </summary>
        /// <param name="user">Entitée UserEntity à supprimer</param>
        /// <exception cref="ArgumentNullException">
        /// Lancée si l'entité UserEntity en paramètre est null
        /// </exception>
        public void DeleteUser(UserEntity user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            m_Context.Users.Remove(user);
        }
    }
}
