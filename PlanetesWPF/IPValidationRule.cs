using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace PlanetesWPF
{
    public class IPValidationRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            //pattern validates if input is IP
            string pat = @"\b(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9])\.
                             (25[0 - 5] | 2[0 - 4][0 - 9] | 1[0 - 9][0 - 9] |[1 - 9]?[0 - 9])\.
                             (25[0 - 5] | 2[0 - 4][0 - 9] | 1[0 - 9][0 - 9] |[1 - 9]?[0 - 9])\.
                             (25[0 - 5] | 2[0 - 4][0 - 9] | 1[0 - 9][0 - 9] |[1 - 9]?[0 - 9])\b";
            Regex rgx = new Regex(pat);

            if (((string)value).Length > 16)
            { return new ValidationResult(false, "Illegal IP"); }

            if (!rgx.IsMatch((string)value))
            {return new ValidationResult(false, "Illegal IP");}
            
            return new ValidationResult(true, null);
        }
    }
}
