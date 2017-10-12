using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using jigsaw.Engine.Enhancement;

namespace jigsaw.Engine
{
    class Membership : AbstractParser
    {
        private static readonly string Boundary = @"^\r*\n*";

        public Dictionary<string, string> Values { get; private set; }

        public Membership(string rawText)
        {
            Raw = rawText;
            Values = new Dictionary<string, string>();
        }

        public override void Parse()
        {
            var lines = Regex.Split(Raw, Boundary, RegexOptions.Multiline)
                .Where(line => !String.IsNullOrEmpty(line)).ToArray();
            foreach (var line in lines)
            {
                var xs = Regex.Matches(line, @"(\S+)(\s|\t|：)+(\S+)").AsEnumerable().SelectMany(x => x).ToArray();
                if (line.Trim().Length > 0 && xs.Length == 0)
                {
                    Values.Add(line.Trim(), String.Empty);
                }
                if (xs.Length > 0)
                {
                    Values.Add(xs[1].Value, xs[3].Value);
                }
            }
        }
    }
}
