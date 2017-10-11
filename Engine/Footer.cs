using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace jigsaw.Engine
{
    class Footer : AbstractParser
    {
        private static readonly string Boundary = @"^\r*\n*";
        private static readonly string ReceiptCodePattern = @"\{#<R>(\d+)#\}";

        // レシート番号
        public string ReceiptCode { get; private set; }

        // 会員情報
        public Membership Info { get; private set; }

        public Footer(string rawText)
        {
            Raw = rawText;
            Info = new Membership();
        }

        public override void Parse()
        {
            ReceiptCode = Regex.Match(Raw, ReceiptCodePattern).Groups[1].Value;

            var lines = Regex.Split(RemovePhrase(), Boundary, RegexOptions.Multiline)
                .Where(line => !String.IsNullOrEmpty(line)).ToArray();
            foreach (var line in lines)
            {
                Console.WriteLine(line);
                var mc = Regex.Matches(line, @"(\S+)(\s|\t|：)+(\S+)");
                if (line.Trim().Length > 0 && mc.Count == 0)
                {
                    Info.Values.Add(line.Trim(), String.Empty);
                }
                foreach (Match m in mc)
                {
                    Info.Values.Add(m.Groups[1].Value, m.Groups[3].Value);
                }
            }
        }

        private string SourceText;
        private string RemovePhrase()
        {
            List<Func<string, string>> xs = new List<Func<string, string>>()
            {
                (s) => { return (new Meta(s)).Payload; },
                (s) => { return Regex.Replace(s, ReceiptCodePattern, String.Empty); },
                (s) => { return Regex.Replace(s, @"\{##\}[\S\s]+\{##\}", "", RegexOptions.Multiline); },
                (s) => { return Regex.Replace(s, @"\+-{12}\+[\S\s]+\+-{12}\+", "", RegexOptions.Multiline); }
            };

            var tmp = String.Copy(Raw);
            foreach (var x in xs)
            {
                tmp = x(tmp);
            }
            SourceText = tmp;
            return tmp;
        }
    }
}
