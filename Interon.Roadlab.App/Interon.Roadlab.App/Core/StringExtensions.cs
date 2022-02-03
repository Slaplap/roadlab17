using System;
using System.Linq;

namespace Interon.Roadlab.App.Core
{
    public static partial class StringExtensions
    {
        public static string RemoveWhiteSpace(this string self)
        {
            return new string(self.Where(c => !Char.IsWhiteSpace(c)).ToArray());
        }
    }
}
