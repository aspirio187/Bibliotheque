using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.StaticData
{
    public enum RolesEnum
    {
        User,
        Moderator,
        Admin,
        SuperAdmin
    }

    public static class RoleData
    {
        /// <summary>
        /// Tableau privé des Roles définis dans le même ordre que
        /// l'enumeration RolesEnum
        /// </summary>
        private static readonly string[] Roles =
        {
            "User",
            "Moderator",
            "Admin",
            "SuperAdmin"
        };

        /// <summary>
        /// Récupère le role en chaine de caractère depuis le tableau
        /// privé de Roles. L'élement de l'enum sert d'indice
        /// </summary>
        /// <param name="role">Role de l'enum RolesEnum</param>
        /// <returns>Chaine de caractère représentant le role</returns>
        public static string GetRole(RolesEnum role) => Roles[(int)role];

        /// <summary>
        /// Récupère l'élement de l'enum RolesEnum qui représente le role
        /// défini en paramètre
        /// </summary>
        /// <param name="role">Chaine de caractère du role désiré</param>
        /// <returns>Le role dans l'enum RolesEnum</returns>
        public static RolesEnum GetRole(string role)
        {
            role = role.Trim().ToLower();
            if (string.IsNullOrEmpty(role))
                throw new ArgumentNullException(nameof(role));

            int roleIndex = Array.IndexOf(Roles, role);
            if (roleIndex < 0)
                throw new ArgumentException(nameof(role));
            return (RolesEnum)roleIndex;
        }
    }
}
