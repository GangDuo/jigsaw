﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace jigsaw.Engine.Component
{
    class Header: AbstractParser
    {
        private static readonly string Boundary = @"^\r*\n*";

        public Meta Meta { get; private set; }
        public string Where { get; private set; }
        public string Address { get; private set; }
        public string Tel { get; private set; }
        public HeaderNames.PosService PosService { get; private set; }
        public HeaderNames.Staff Staff { get; private set; }

        public Header(string rawHeader)
        {
            Raw = rawHeader;
        }

        public override void Parse()
        {
            Meta = new Meta(Raw);
            var lines = Regex.Split(Meta.Payload, Boundary, RegexOptions.Multiline)
                .Where(line => !String.IsNullOrEmpty(line)).ToArray();

            Debug.Assert(lines.Length == 6);
            Where = lines[0].Trim();
            Address = lines[1].Trim();
            Tel = lines[2].Trim();

            PosService = new HeaderNames.PosService(lines[4]);
            Staff = new HeaderNames.Staff(lines[5]);
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
