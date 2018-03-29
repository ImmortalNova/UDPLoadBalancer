using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Configurator
{
    public static class Extensions
    {
        public static bool IsNumeric(this String input)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(input);
        }
    }
}
