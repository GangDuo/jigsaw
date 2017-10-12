using System;
using System.Text.RegularExpressions;
using System.Linq;
using jigsaw.Engine.Enhancement;

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
            var xs = Regex.Matches(fields[1], @"^(\S+)(\s|\t)+\(@(\S+)×(\S+)\)").AsEnumerable().SelectMany(x => x).ToArray();
            if (xs.Length > 0)
            {
                Jan = xs[1].Value;
                Price = int.Parse(xs[3].Value.Replace(",", ""));
                Qty = int.Parse(xs[4].Value);
            }
            Name = fields[0].Trim();

            xs = Regex.Matches(fields[2], @"(\S*)\s+\\(\S+)").AsEnumerable().SelectMany(x => x).ToArray();
            if (xs.Length > 0)
            {
                DiscountRate = xs[1].Value;
                Total = Decimal.Parse(xs[2].Value);
            }
        }
    }
}
