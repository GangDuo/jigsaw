using System.Text;

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

            var expression = new Helpers.Expression(sources);
            (new Text.Csv()).ConvertDataTableToCsv(expression.ToDataTable(), "out.csv", true);
        }
    }
}
