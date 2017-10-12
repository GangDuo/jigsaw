using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using jigsaw.Engine.Enhancement;

namespace jigsaw.Engine
{
    class Total : AbstractParser
    {
        private static readonly string Boundary = @"^\r*\n*";
        public Dictionary<string, string> Values { get; private set; }

        public Total(string rawText)
        {
            Raw = rawText;
            Values = new Dictionary<string, string>();
        }

        public override void Parse()
        {
            var lines = Regex.Split(Raw, Boundary, RegexOptions.Multiline).Where(line => !String.IsNullOrEmpty(line)).ToArray();
            foreach (var line in lines)
            {
                var xs = Regex.Matches(line, @"(\S+)(\s|\t)+(\S+)").AsEnumerable().SelectMany(x => x).ToArray();
                if(xs.Length > 0)
                {
                    Debug.Assert(xs.Length == 4);
                    Values.Add(xs[1].Value.TrimStart(new char[] { '(' }),
                               xs[3].Value.TrimEnd(new char[] { ')' }));
                }
            }
        }
    }
}
