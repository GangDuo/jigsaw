using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using jigsaw.Engine.Component;

namespace jigsaw.UnitTest
{
    /// <summary>
    /// UnitTest1 の概要の説明
    /// </summary>
    [TestClass]
    public class ReceiptUnitTests
    {
        public ReceiptUnitTests()
        {
            //
            // TODO: コンストラクター ロジックをここに追加します
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        //
        // テストを作成する際には、次の追加属性を使用できます:
        //
        // クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext)
        // {
        //     System.Diagnostics.Trace.WriteLine("ClassInitialize");
        // }
        //
        // クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void 練習モードレシート()
        {
            var instance = new Receipt(System.IO.File.ReadAllText(@"DATA\練習.txt"));
            instance.Parse();
            Assert.IsTrue(instance.Header.Meta.IsPractice);
            Assert.IsFalse(instance.Header.Meta.IsRePrint);
            Assert.IsFalse(instance.Header.Meta.IsReturn);
        }

        [TestMethod]
        public void 再印刷レシート()
        {
            var instance = new Receipt(System.IO.File.ReadAllText(@"DATA\再印刷.txt"));
            instance.Parse();
            Assert.IsFalse(instance.Header.Meta.IsPractice);
            Assert.IsTrue(instance.Header.Meta.IsRePrint);
            Assert.IsFalse(instance.Header.Meta.IsReturn);
        }

        [TestMethod]
        public void 返品レシート()
        {
            var instance = new Receipt(System.IO.File.ReadAllText(@"DATA\返品.txt"));
            instance.Parse();
            Assert.IsFalse(instance.Header.Meta.IsPractice);
            Assert.IsFalse(instance.Header.Meta.IsRePrint);
            Assert.IsTrue(instance.Header.Meta.IsReturn);
        }

        [TestMethod]
        public void 通常レシート()
        {
            var instance = new Receipt(System.IO.File.ReadAllText(@"DATA\通常.txt"));
            instance.Parse();
            Assert.IsFalse(instance.Header.Meta.IsPractice);
            Assert.IsFalse(instance.Header.Meta.IsRePrint);
            Assert.IsFalse(instance.Header.Meta.IsReturn);
            Assert.AreEqual("太田飯塚店", instance.Header.Where);
            Assert.AreEqual("群馬県太田市飯塚町801-1", instance.Header.Address);
            Assert.AreEqual("TEL:045-640-5633", instance.Header.Tel);
            Assert.AreEqual(new DateTime(2017, 4, 15, 18, 59, 50), instance.Header.PosService.When);
            Assert.AreEqual("007", instance.Header.PosService.ShopCode);
            Assert.AreEqual("111", instance.Header.PosService.PosCode);
            Assert.AreEqual("0941", instance.Header.Staff.Code);

            Assert.AreEqual(14, instance.Goods.Collection.Count);
            {
                var goods = instance.Goods.Collection[0];
                Assert.AreEqual("TOMSｳｨﾒﾝｽﾞ ｼｰｽﾞﾅﾙｸﾗｼｯｸBK ﾓﾛｯｺｸﾛｼｪｯﾄ-6", goods.Name);
                Assert.AreEqual("0889556003204", goods.Jan);
                Assert.AreEqual(9504, goods.Price);
                Assert.AreEqual(1, goods.Qty);
                Assert.AreEqual(9004, goods.Total);
                Assert.AreEqual(@"(値引：\-500)", goods.DiscountRate);
            }
            {
                var goods = instance.Goods.Collection[13];
                Assert.AreEqual("ﾘﾎﾞﾝｽﾘｰﾌﾞﾀｯｸﾌﾞﾗｳｽ OFF", goods.Name);
                Assert.AreEqual("0600000510327", goods.Jan);
                Assert.AreEqual(5292, goods.Price);
                Assert.AreEqual(1, goods.Qty);
                Assert.AreEqual(5292, goods.Total);
                Assert.AreEqual("", goods.DiscountRate);
            }

            CollectionAssert.AreEqual(new Dictionary<string, string>()
            {
                {"お買上点数", "14"},
                {"小計",  @"\71,820"},
                {"合計",  @"\71,820"},
                {"内税",  @"\5,320"},
            }, instance.Total.Values);

            CollectionAssert.AreEqual(new Dictionary<string, string>()
            {
                {"カード", ""},
                {"クレジット", @"\71,820"},
                {"お釣り", @"\0"},
            }, instance.Deposit.Values);

            Assert.AreEqual("0000047547", instance.Footer.ReceiptCode);
            CollectionAssert.AreEqual(new Dictionary<string, string>()
            {
                {"【返品・交換について】", ""},
                {"会員番号", "9999999999999"},
                {"ポイント", ""},
                {"現在ポイント", "351P"},
                {"今回加算分", "718P"},
                {"累計ポイント", "1,069P"},
            }, instance.Footer.Info.Values);
        }
    }
}
