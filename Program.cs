using System.Text;
using jigsaw.Helpers;

namespace jigsaw
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.ReceiptCollection sources = null;
            using (var sr = new System.IO.StreamReader("Sale.txt", Encoding.UTF8))
            {
                sources = new Engine.ReceiptCollection(sr.ReadToEnd());
                sources.Parse();
            }

            (new Text.Csv()).ConvertDataTableToCsv(sources.ToDataTable(), "out.csv", true);
        }
    }
}
