using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jigsaw.Helpers
{
    static class Extensions
    {
        public static System.Data.DataTable ToDataTable(this Engine.ReceiptCollection source)
        {
            var table = new System.Data.DataTable();
            table.Columns.AddRange(new System.Data.DataColumn[]
            {
                new System.Data.DataColumn("レシート番号", typeof(String)),
                new System.Data.DataColumn("住所",         typeof(String)),
                new System.Data.DataColumn("電話番号",     typeof(String)),
                new System.Data.DataColumn("店舗名",       typeof(String)),
                new System.Data.DataColumn("日時",         typeof(DateTime)),
                new System.Data.DataColumn("店舗コード",   typeof(String)),
                new System.Data.DataColumn("POS",          typeof(String)),
                new System.Data.DataColumn("スタッフ",     typeof(String)),
                new System.Data.DataColumn("返品",         typeof(Boolean)),
                new System.Data.DataColumn("品名",         typeof(String)),
                new System.Data.DataColumn("JAN",          typeof(String)),
                new System.Data.DataColumn("上代",         typeof(Int32)),
                new System.Data.DataColumn("数量",         typeof(Int32)),
                new System.Data.DataColumn("商品合計",     typeof(Decimal)),
                new System.Data.DataColumn("商品割引",     typeof(String)),
                new System.Data.DataColumn("お買上点数",   typeof(String)),
                new System.Data.DataColumn("小計",         typeof(String)),
                new System.Data.DataColumn("小計割引",     typeof(String)),
                new System.Data.DataColumn("合計",         typeof(String)),
                new System.Data.DataColumn("内税",         typeof(String)),
                new System.Data.DataColumn("お預かり",     typeof(String)),
                new System.Data.DataColumn("お釣り",       typeof(String)),
            });

            foreach (var receipt in source.Collection)
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
                    row["日時"] = receipt.Header.When;
                    row["店舗コード"] = receipt.Header.ShopCode;
                    row["POS"] = receipt.Header.PosCode;
                    row["スタッフ"] = receipt.Header.StaffCode;
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
