using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace jigsaw.Engine
{
    class Deposit : AbstractParser
    {
        private static readonly string Boundary = @"^\r*\n*";
        public Dictionary<string, string> Values { get; private set; }

        public Deposit(string rawText)
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
                var mc = Regex.Matches(line, @"(\S+)(\s|\t)+(\S+)");
                if (mc.Count == 0)
                {
                    Values.Add(line.Trim(), "");
                }
                foreach (Match m in mc)
                {
                    Values.Add(m.Groups[1].Value, m.Groups[3].Value);
                }
            }
        }
    }
}
