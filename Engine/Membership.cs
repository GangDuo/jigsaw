using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

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
                Console.WriteLine(line);
                var mc = Regex.Matches(line, @"(\S+)(\s|\t|：)+(\S+)");
                if (line.Trim().Length > 0 && mc.Count == 0)
                {
                    Values.Add(line.Trim(), String.Empty);
                }
                foreach (Match m in mc)
                {
                    Values.Add(m.Groups[1].Value, m.Groups[3].Value);
                }
            }
        }
    }
}
