using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace jigsaw.Engine
{
    class Header: AbstractParser
    {
        private static readonly string Boundary = @"^\r*\n*";

        public bool IsPractice { get; private set; }
        public bool IsRePrint { get; private set; }
        public bool IsReturn { get; private set; }
        public string Where { get; private set; }
        public string Address { get; private set; }
        public string Tel { get; private set; }
        public DateTime When { get; private set; }
        public string ShopCode { get; private set; }
        public string PosCode { get; private set; }
        public string StaffCode { get; private set; }

        public Header(string rawHeader)
        {
            Raw = rawHeader;
        }

        public override void Parse()
        {
            var meta = new Meta(Raw);
            IsPractice = meta.IsPractice();
            IsRePrint  = meta.IsRePrint();
            IsReturn = meta.IsReturn();       

            var lines = Regex.Split(meta.Payload, Boundary, RegexOptions.Multiline)
                .Where(line => !String.IsNullOrEmpty(line)).ToArray();

            Debug.Assert(lines.Length == 6);
            Where = lines[0].Trim();
            Address = lines[1].Trim();
            Tel = lines[2].Trim();

            var info = new Info(lines[4]);
            info.Parse();
            When = info.When;
            ShopCode = info.ShopCode;
            PosCode = info.PosCode;

            var staff = new Staff(lines[5]);
            staff.Parse();
            StaffCode = staff.Code;
        }

        private class Staff : AbstractParser
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

        private class Info: AbstractParser
        {
            private static readonly string Boundary = @"\s+";

            public DateTime When { get; private set; }
            public string ShopCode { get; private set; }
            public string PosCode { get; private set; }

            public Info(string rawText)
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
}
