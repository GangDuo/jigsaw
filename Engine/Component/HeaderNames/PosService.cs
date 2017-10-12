using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace jigsaw.Engine.Component.HeaderNames
{
    class PosService : AbstractParser
    {
        private static readonly string Boundary = @"\s+";

        public DateTime When { get; private set; }
        public string ShopCode { get; private set; }
        public string PosCode { get; private set; }

        public PosService(string rawText)
        {
            Raw = rawText;
        }

        public override void Parse()
        {
            var values = Regex.Split(Raw, Boundary).Where(x => !String.IsNullOrEmpty(x)).ToArray();
            When = DateTime.Parse(String.Format("{0} {1}", values[0], values[1]));
            ShopCode = values[2];
            PosCode = values[3];
        }
    }
}
