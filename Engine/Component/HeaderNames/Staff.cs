using System;
using System.Diagnostics;

namespace jigsaw.Engine.Component.HeaderNames
{
    class Staff : AbstractParser
    {
        private static readonly string Boundary = "：";

        public string Code { get; private set; }

        public Staff(string rawText)
        {
            Raw = rawText;
        }

        public override void Parse()
        {
            var splited = Raw.Split(new string[] { Boundary }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert(splited.Length == 2);
            Code = splited[1].Trim();
        }
    }
}
