using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace jigsaw.Engine.Enhancement
{
    static class ForRegex
    {
        public static IEnumerable<IEnumerable<Group>> AsEnumerable(this MatchCollection xs)
        {
            return xs.Cast<Match>().Select(x => x.Groups.Cast<Group>());
        }
    }
}
