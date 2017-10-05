using System;
using System.Collections.Generic;

namespace jigsaw.Engine
{
    class GoodsCollection : AbstractParser
    {
        private static readonly string Boundary = "\r\r\n\r\r\n";
        public List<Goods> Collection { get; private set; }

        public GoodsCollection(string rawText)
        {
            Raw = rawText;
            Collection = new List<Goods>();
        }

        public override void Parse()
        {
            var lines = Raw.Split(new string[] { Boundary }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var instance = new Goods(line);
                instance.Parse();
                Collection.Add(instance);
            }
        }
    }
}
