using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MathBlaster
{
    public static class ExtentsionMethods
    {
        public static decimal ToDecimal(this String str)
        {
            var result = 0m;
            return decimal.TryParse(str, out result) ? result : 0;
        }

        public static string BackSpace(this String str)
        {
            var result = string.Empty;

            if (str.Length > 0)
            {
                result = str.Substring(0, str.Length - 1);
            }

            return result;
        }
    }
}
