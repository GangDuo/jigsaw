using System;
using System.Text.RegularExpressions;
using System.Linq;
using jigsaw.Engine.Enhancement;

namespace jigsaw.Engine.Component
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
            Name = fields[0].Trim();
            Jan = fields[1].Trim();
            // 上代、数量、商品割引
            var xs = Regex.Matches(fields[fields.Length - 3], @"^\(@(\S+)×(\S+)\)(\s+\(割引：(\S*)\))?").AsEnumerable().SelectMany(x => x).ToArray();
            if (xs.Length > 0)
            {
                Price = int.Parse(xs[1].Value.Replace(",", ""));
                Qty = int.Parse(xs[2].Value);
                DiscountRate = xs[4].Value;
            }
            // 商品合計
            xs = Regex.Matches(fields[fields.Length - 2], @"\s+\\(\S+)").AsEnumerable().SelectMany(x => x).ToArray();
            if (xs.Length > 0)
            {
                Total = Decimal.Parse(xs[1].Value);
            }
        }
    }
}
