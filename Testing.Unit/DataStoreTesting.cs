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
                "0000000000000000000000000000000",
                0);
            var block = new Block();
            block.Data = "test";
            TimeSpan ts = new DateTime(2015, 2, 14, 3, 22, 13)
                .Subtract(new DateTime(1970, 1, 1));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            dataStore.CalculateHash(block,
                0);
            block.ThisHash.Should().Be("b159409daaab04d3669973a61bf98715c6975c913321c41a8fb2c76a54121d5a");

            block = new Block();
            block.Data = "test2";
            ts = new DateTime(2015, 2, 14, 3, 22, 13).Subtract(new DateTime(1970, 1, 1));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            dataStore.CalculateHash(block,
                0);
            block.ThisHash.Should().Be("7bec0d90805220d228fa51bf9f0907a69088c6246a003f3fa66a92fa32128278");

            block = new Block();
            block.Data = "test";
            ts = new DateTime(2015, 2, 14, 3, 22, 13)
                .Subtract(new DateTime(1970, 1, 1));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            dataStore.CalculateHash(block,
                4);
            block.ThisHash.Should().Be("000068a4fb51099a1f6132557f3ee053770afba130e7728cae82552815fb473e");
            block.Nonce.Should().Be(43300);
        }

        [TestMethod]
        public void TryAdd()
        {
            var dataStore = new DataStore("",
                0,
                "0000000000000000000000000000000",
                0);
            var block = new Block();
            block.Data = "test";
            dataStore.TryAdd(block,
                0);

            block = new Block()
            {
                Data = "test2block"
            };

            dataStore.TryAdd(block,
                0);

            dataStore.ChainLinks.Should().NotBeNullOrEmpty();
            dataStore.ChainLinks.Count.Should().Be(3);
        }
    }
}