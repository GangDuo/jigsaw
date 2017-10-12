using System;
using System.Collections.Generic;
using jigsaw.Engine.Component;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jigsaw.Engine
{
    class ReceiptCollection : AbstractParser
    {
        private static readonly string Boundary = "=======================================================\r\n";
        public List<Receipt> Collection { get; private set; }

        public ReceiptCollection(string rawText)
        {
            Raw = rawText;
            Collection = new List<Receipt>();
        }

        public override void Parse()
        {
            var xs = Raw.Split(new string[] { Boundary }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var x in xs)
            {
                var receipt = new Receipt(x);
                receipt.Parse();
                Collection.Add(receipt);
            }
        }
    }
}
