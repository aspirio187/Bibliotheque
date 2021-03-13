using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.EntityFramework.Helpers
{
    public enum BookState
    {
        Vide,
        Neuf,
        CommeNeuf,
        Excellent,
        TresBon,
        Bon,
        Acceptable,
        Usage,
        HorsUsage
    }

    public static class BookHelper
    {
        private static readonly string[] State =
        {
            "",
            "Neuf",
            "Comme neuf",
            "Excellent état",
            "Très bon état",
            "Bon",
            "Acceptable",
            "Usé",
            "Hors d'usage"
        };

        /// <summary>
        /// Méthode d'extension pour l'enum BookState. Converti l'élement de l'enum en 
        /// son élément string
        /// </summary>
        /// <param name="state">Element de l'enum BookState</param>
        /// <returns>Chaine de caractère (string) représentant l'état</returns>
        public static string StateToString(this BookState state) => State[(int)state];

        /// <summary>
        /// Méthode d'extension pour les string. Converti le string en paramètre en son
        /// élement dans l'enum BookState. Si aucun élément n'est trouvé, la méthode
        /// retourne HorsUsage
        /// </summary>
        /// <param name="stateString"></param>
        /// <returns>Element de l'enum BookState associé au string en param</returns>
        public static BookState StateToEnum(this string stateString)
        {
            for (int i = 0; i < State.Length; i++)
            {
                if (State[i].Equals(stateString))
                    return (BookState)i;
            }
            return BookState.HorsUsage;
        }
    }
}
