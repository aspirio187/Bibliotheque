using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Helpers
{
    public static class StringExtension
    {
        public static string FirstCharToUpper(this string input) => input switch
        {
            null or "" => string.Empty,
            _ => input.Trim().First().ToString().ToUpper() + input.Substring(1).Trim(),
        };
    }
}
