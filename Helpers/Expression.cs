using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jigsaw.Helpers
{
    class Expression
    {
        private Engine.ReceiptCollection Source;

        public Expression(Engine.ReceiptCollection source)
        {
            Source = source;
        }

        public DataTable ToDataTable()
        {
            var table = new DataTable();
            table.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("レシート番号", typeof(String)),
                new DataColumn("住所",         typeof(String)),
                new DataColumn("電話番号",     typeof(String)),
                new DataColumn("店舗名",       typeof(String)),
                new DataColumn("日時",         typeof(DateTime)),
                new DataColumn("店舗コード",   typeof(String)),
                new DataColumn("POS",          typeof(String)),
                new DataColumn("スタッフ",     typeof(String)),
                new DataColumn("返品",         typeof(Boolean)),
                new DataColumn("品名",         typeof(String)),
                new DataColumn("JAN",          typeof(String)),
                new DataColumn("上代",         typeof(Int32)),
                new DataColumn("数量",         typeof(Int32)),
                new DataColumn("商品合計",     typeof(Decimal)),
                new DataColumn("商品割引",     typeof(String)),
                new DataColumn("お買上点数",   typeof(String)),
                new DataColumn("小計",         typeof(String)),
                new DataColumn("小計割引",     typeof(String)),
                new DataColumn("合計",         typeof(String)),
                new DataColumn("内税",         typeof(String)),
                new DataColumn("お預かり",     typeof(String)),
                new DataColumn("お釣り",       typeof(String)),
            });

            foreach (var receipt in Source.Collection)
            {
                if (receipt.Header.IsPractice || receipt.Header.IsRePrint) { continue; }
                foreach (var k in receipt.Total.Values.Keys)
                {
                    if (!table.Columns.Contains(k))
                    {
                        table.Columns.Add(new System.Data.DataColumn(k, typeof(String)));
                    }
                }
                foreach (var k in receipt.Deposit.Values.Keys)
                {
                    if (!table.Columns.Contains(k))
                    {
                        table.Columns.Add(new System.Data.DataColumn(k, typeof(String)));
                    }
                }
                foreach (var k in receipt.Footer.Info.Values.Keys)
                {
                    if (!table.Columns.Contains(k))
                    {
                        table.Columns.Add(new System.Data.DataColumn(k, typeof(String)));
                    }
                }

                foreach (var x in receipt.Goods.Collection)
                {
                    var row = table.NewRow();
                    row["返品"] = receipt.Header.IsReturn;
                    row["店舗名"] = receipt.Header.Where;
                    row["住所"] = receipt.Header.Address;
                    row["電話番号"] = receipt.Header.Tel;
                    row["日時"] = receipt.Header.PosService.When;
                    row["店舗コード"] = receipt.Header.PosService.ShopCode;
                    row["POS"] = receipt.Header.PosService.PosCode;
                    row["スタッフ"] = receipt.Header.Staff.Code;
                    row["レシート番号"] = receipt.Footer.ReceiptCode;
                    row["品名"] = x.Name;
                    row["JAN"] = x.Jan;
                    row["上代"] = x.Price;
                    row["数量"] = x.Qty;
                    row["商品合計"] = x.Total;
                    row["商品割引"] = x.DiscountRate;

                    foreach (var k in receipt.Total.Values.Keys)
                    {
                        row[k] = receipt.Total.Values[k];
                    }
                    foreach (var k in receipt.Deposit.Values.Keys)
                    {
                        row[k] = receipt.Deposit.Values[k];
                    }
                    foreach (var k in receipt.Footer.Info.Values.Keys)
                    {
                        row[k] = receipt.Footer.Info.Values[k];
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }
    }
}
