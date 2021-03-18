using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Helpers
{
    public static class ErrorClaim
    {
        public static void RaiseError(string message, params object[] args)
        {
            string errorMessage = $"An error occured : {message}";
            Debug.WriteLine(errorMessage);
            Debug.WriteLine(new String('-', errorMessage.Length));
            foreach (object item in args)
            {
                Debug.WriteLine($"\t* { item }");
            }
        }
    }
}
