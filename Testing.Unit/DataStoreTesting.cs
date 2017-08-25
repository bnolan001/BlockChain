using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockChain.NET.Library;
using FluentAssertions;
using System.Text;

namespace Testing.Unit
{
    [TestClass]
    public class DataStoreTesting
    {
        [TestMethod]
        public void CalculateHash()
        {
            var dataStore = new DataStore("",
                0,
                "0000000000000000000000000000000");
            var block = new Block();
            block.Data = "test";
            TimeSpan ts = new DateTime(2015, 2, 14, 3, 22, 13).Subtract(new DateTime(1970, 1, 1));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            var hash = dataStore.CalculateHash(block);
            hash.Should().Be("±Y@\u009dª«\u0004Óf™s¦\u001bù‡\u0015Æ—\\‘3!Ä\u001a\u008f²ÇjT\u0012\u001dZ");

            block = new Block();
            block.Data = "test2";
            ts = new DateTime(2015, 2, 14, 3, 22, 13).Subtract(new DateTime(1970, 1, 1));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            hash = dataStore.CalculateHash(block);
            hash.Should().Be("{ì\r\u0090€R Ò(úQ¿Ÿ\t\a¦\u0090ˆÆ$j\0??¦j’ú2\u0012‚x");
        }

        [TestMethod]
        public void TryAdd()
        {
            var dataStore = new DataStore("",
                0,
                "0000000000000000000000000000000");
            var block = new Block();
            block.Data = "test";
            dataStore.TryAdd(block);

            block = new Block()
            {
                Data = "test2block"
            };

            dataStore.TryAdd(block);

            dataStore.ChainLinks.Should().NotBeNullOrEmpty();
            dataStore.ChainLinks.Count.Should().Be(3);
        }
    }
}