using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kzta.Extensions
{
    public static class Extension
    {
        public static int ParseToInt(this string text)
        {
            if (int.TryParse(text, out var result))
            {
                return result;
            }

            return 0;
        }

    }
}
