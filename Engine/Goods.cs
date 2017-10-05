using System;
using System.Text.RegularExpressions;

namespace jigsaw.Engine
{
    class Goods : AbstractParser
    {
        private static readonly string Boundary = "\r\r\n";

        public string Name { get; private set; }
        public string Jan { get; private set; }
        public int Price { get; private set; }
        public int Qty { get; private set; }
        public decimal Total { get; private set; }
        public string DiscountRate { get; private set; }

        public Goods(string rawText)
        {
            Raw = rawText;
        }

        public override void Parse()
        {
            var fields = Raw.Split(new string[] { Boundary }, StringSplitOptions.RemoveEmptyEntries);
            var mc = Regex.Matches(fields[1], @"^(\S+)(\s|\t)+\(@(\S+)×(\S+)\)");
            foreach (Match m in mc)
            {
                Jan = m.Groups[1].Value;
                Price = int.Parse(m.Groups[3].Value.Replace(",", ""));
                Qty = int.Parse(m.Groups[4].Value);
            }
            Name = fields[0].Trim();

            mc = Regex.Matches(fields[2], @"(\S*)\s+\\(\S+)");
            foreach (Match m in mc)
            {
                DiscountRate = m.Groups[1].Value;
                Total = Decimal.Parse(m.Groups[2].Value);
            }
        }
    }
}
