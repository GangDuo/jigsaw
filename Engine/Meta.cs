using System.Reflection;
using System.Text.RegularExpressions;

namespace jigsaw.Engine
{
    class Meta
    {
        private static readonly string PracticePattern = @"\*+\s*練習モード\s*\*+";
        private static readonly string RePrintPattern = @"\*+\s*再印刷\s*\*+";
        private static readonly string ReturnPattern = @"\*+\s*返品\s*\*+";

        public string Raw { get; private set; }
        public string Payload { get; private set; }

        public Meta(string rawText)
        {
            Raw = rawText;

            var tmp = string.Copy(Raw);
            var xs = typeof(Meta).GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField);
            foreach (FieldInfo info in xs)
            {
                var pattern = info.GetValue(null).ToString();
                tmp = Regex.Replace(tmp, pattern, "");
            }
            Payload = tmp;
        }

        public bool IsPractice()
        {
            return Regex.IsMatch(Raw, PracticePattern);
        }

        public bool IsRePrint()
        {
            return Regex.IsMatch(Raw, RePrintPattern);
        }

        public bool IsReturn()
        {
            return Regex.IsMatch(Raw, ReturnPattern);
        }
    }
}
