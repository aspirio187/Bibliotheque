using System;
using System.Collections.Generic;
using System.IO;
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

        public static string FirstCharToUpperForAll(this string input, char seperator)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string[] vs = input.Split(seperator);
            for (int i = 0; i < vs.Length; i++)
            {
                vs[i] = vs[i].FirstCharToUpper();
                if ((i + 1) < vs.Length) { stringBuilder.Append($"{vs[i]}{seperator}"); }
                else { stringBuilder.Append(vs[i]); }
            }
            return stringBuilder.ToString();
        }

        // TODO : supprimer la présentation
        // Jean-Charles Lemonnier
        // seperator = { ' ', '-' }
        // 1er parcours :
        // output = jean-charles lemonnier
        // vs = { jean-charles, lemonnier }
        // vs = { Jean-charles Lemonnier }
        // output = Jean-charles Lemonnier
        // 2ème parcours :
        // output = Jean-charles Lemonnier
        // vs = { Jean, charles Lemonnier }
        // vs = { Jean, Charles lemonnier }
        // output = Jean-Charles Lemonnier
        public static string FirstCharToUpperForAll(this string input, char[] seperator)
        {
            string output = input;
            for (int i = 0; i < seperator.Length; i++)
            {
                output = FirstCharToUpperForAll(output, seperator[i]);
            }
            return output;
        }

        public static bool IsDigit(this string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i])) return false;
            }
            return true;
        }

    }
}
