using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace jigsaw.Engine.Component
{
    class Footer : AbstractParser
    {
        private static readonly string ReceiptCodePattern = @"\{#<R>(\d+)#\}";

        // レシート番号
        public string ReceiptCode { get; private set; }

        // 会員情報
        public Membership Info { get; private set; }

        public Footer(string rawText)
        {
            Raw = rawText;
        }

        public override void Parse()
        {
            ReceiptCode = Regex.Match(Raw, ReceiptCodePattern).Groups[1].Value;

            List<Func<string, string>> xs = new List<Func<string, string>>()
            {
                (s) => { return (new Meta(s)).Payload; },
                (s) => { return Regex.Replace(s, ReceiptCodePattern, String.Empty); },
                (s) => { return Regex.Replace(s, @"\{##\}[\S\s]+\{##\}", "", RegexOptions.Multiline); },        // 広告
                (s) => { return Regex.Replace(s, @"\+-{12}\+[\S\s]+\+-{12}\+", "", RegexOptions.Multiline); }   // 収入印紙
            };
            var payload = String.Copy(Raw);
            foreach (var x in xs)
            {
                payload = x(payload);
            }

            Info = new Membership(payload);
            Info.Parse();
        }
    }
}
