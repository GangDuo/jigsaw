using System.Reflection;
using System.Text.RegularExpressions;

namespace jigsaw.Engine
{
    class Meta
    {
        private static readonly string PracticePattern = @"\*+\s*練習モード\s*\*+";
        private static readonly string RePrintPattern = @"\*+\s*再印刷\s*\*+";
        private static readonly string ReturnPattern = @"\*+\s*返品\s*\*+";

        public static string RemoveMetaPhrase(string rawText)
        {
            var tmp = rawText;
            var xs = typeof(Meta).GetFields(BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.GetField);
            foreach (FieldInfo info in xs)
            {
                var pattern = info.GetValue(null).ToString();
                tmp = Regex.Replace(tmp, pattern, "");
            }
            return tmp;
        }

        public static bool IsPractice(string rawText)
        {
            return Regex.IsMatch(rawText, PracticePattern);
        }

        public static bool IsRePrint(string rawText)
        {
            return Regex.IsMatch(rawText, RePrintPattern);
        }

        public static bool IsReturn(string rawText)
        {
            return Regex.IsMatch(rawText, ReturnPattern);
        }
    }
}
