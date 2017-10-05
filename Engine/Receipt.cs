using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace jigsaw.Engine
{
    class Receipt: AbstractParser
    {
        private static readonly string Boundary = @"------------------------------------------------\r*\n*";

        public Header Header { get; private set; }
        public GoodsCollection Goods { get; private set; }
        public Total Total { get; private set; }
        public Deposit Deposit { get; private set; }
        public Footer Footer { get; private set; }

        public Receipt(string rawText)
        {
            Raw = rawText;
        }

        public override void Parse()
        {
            var splited = Regex.Split(Raw, Boundary);

            Debug.Assert(splited.Length == 5);
            Header = new Header(splited[0]);            // ヘッダ
            Goods = new GoodsCollection(splited[1]);    // 購入商品
            Total = new Total(splited[2]);              // 明細合計金額
            Deposit = new Deposit(splited[3]);          // 預かり金額＆お釣り
            Footer = new Footer(splited[4]);            // フッタ

            foreach (PropertyInfo info in this.GetType().GetProperties())
            {
                var value = info.GetValue(this);
                if (value is AbstractParser)
                {
                    (value as AbstractParser).Parse();
                }
            }
        }
    }
}
