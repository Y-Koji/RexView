using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RexView.Model
{
    public static class RegexExtension
    {
        public static RegexOptions Aggregate(this IEnumerable<RegexOption> self)
        {
            IList<RegexOptions> options = new List<RegexOptions>();

            foreach (var option in self)
            {
                if (option.IsChecked)
                {
                    options.Add(option.OptionValue);
                }
            }

            if (0 == options.Count)
            {
                options.Add(RegexOptions.None);
            }

            return options.Aggregate((option1, option2) => option1 | option2);
        }
    }
}
