using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Helpers
{
    public enum BookStates
    {
        Unusable,
        Worn,
        Acceptable,
        Good,
        VeryGood,
        Excellent,
        LikeNew,
        New
    }

    public static class BookHelper
    {
        public static readonly string[] States =
        {
            "Inutilisable",
            "Usage",
            "Acceptable",
            "Bon",
            "Très bon",
            "Excellent",
            "Comme neuf",
            "Neuf"
        };

        public static bool StateExist(string state)
        {
            if (state is null) throw new ArgumentNullException(nameof(state));
            if (state.Length < 1) throw new ArgumentException($"The parameter state is empty : {nameof(state)}");
            return States.Contains(state);
        }

        public static string GetState(BookStates bookState)
        {
            return States[(int)bookState];
        }

        public static BookStates GetState(string state)
        {
            if (state is null) throw new ArgumentNullException(nameof(state));
            if (state.Length < 1) throw new ArgumentException($"The parameter state is empty : {nameof(state)}");
            if (!States.Contains(state)) throw new ArgumentException($"The parameter state does not exist : {nameof(state)}");
            int stateIndex = Array.IndexOf(States, state);
            if (stateIndex < 0) throw new ArgumentException($"The parameter state could not be found in the states : {nameof(States)}");
            return (BookStates)stateIndex;
        }
    }
}
