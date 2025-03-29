
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_DAM.Utils
{
    class StringUtils
    {
        public static int? ConvertToNumber(string str)
        {
            if (!int.TryParse(str, out int val))
            {
                return null;
            }
            return val;
        }

        public static int ConvertToNumberNoPanic(string str)
        {
            if (!int.TryParse(str, out int val))
            {
                return 0;
            }
            return val;
        }
    }
}
