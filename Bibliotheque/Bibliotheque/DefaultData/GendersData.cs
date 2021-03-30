using Bibliotheque.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.DefaultData
{
    public static class GendersData
    {
        public static List<string> GetGenders()
        {
            return new()
            {
                "Homme",
                "Femme"
            };
        }
    }
}
