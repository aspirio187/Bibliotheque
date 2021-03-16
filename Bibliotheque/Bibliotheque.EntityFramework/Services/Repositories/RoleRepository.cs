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
        Task<bool> RoleExists(Guid roleId);
        Task<RoleEntity> GetRole(string roleName);
        Task<RoleEntity> GetRole(Guid roleId);
    }

    // TODO : Vérifier s'il est utile de lancer des exception depuis le repository si on ne trouve pas le role
    // La manière la plus intelligente serait peut-être de simplement retourner null
    public partial class LibraryRepository : ILibraryRepository
    {
        /// <summary>
        /// Vérifie s'il existe un role dans le contexte ayant l'id en paramètre
        /// </summary>
        /// <param name="roleId">ID du role recherché</param>
        /// <returns>
        /// true Si le role existe. false Dans le cas contraire
        /// </returns>
        public async Task<bool> RoleExists(Guid roleId)
        {
            return await m_Context.Roles.AnyAsync(x => x.Id == roleId);
        }

        /// <summary>
        /// Récupère le role ayant comme nom la chaine de caractères en paramètre
        /// du contexte. Si la chaine est null ou vide ou si le role est introuvable
        /// dans le contexte, le programme lance exception
        /// </summary>
        /// <param name="roleName">
        /// Chaine de caractère représentant le nom du role recherché
        /// </param>
        /// <returns>
        /// L'entité RoleEntity du contexte
        /// </returns>
        public async Task<RoleEntity> GetRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) throw new ArgumentNullException(nameof(roleName));
            var role = await m_Context.Roles.FirstOrDefaultAsync(x => x.Name == roleName);
            if (role == null) throw new ArgumentNullException(nameof(role));
            return role;
        }

        /// <summary>
        /// Récupère le role ayant comme ID le Guid en paramètre du contexte. Si le Guid
        /// est vide ou si le role est introuvable dans le contexte, le programme lance
        /// une exception
        /// </summary>
        /// <param name="roleId">
        /// ID du role que l'utilisateur cherche
        /// </param>
        /// <returns>
        /// L'entité RoleEntity du contexte
        /// </returns>
        public async Task<RoleEntity> GetRole(Guid roleId)
        {
            if (roleId == Guid.Empty) throw new ArgumentNullException(nameof(roleId));
            var role = await m_Context.Roles.FirstOrDefaultAsync(x => x.Id == roleId);
            if (role == null) throw new ArgumentNullException(nameof(role));
            return role;
        }
    }
}
