using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlockChain.NET.Library;
using FluentAssertions;

namespace Testing.Unit
{
    [TestClass]
    public class DataStoreTesting
    {
        [TestMethod]
        public void CalculateHash()
        {
            var dataStore = new DataStore<string>();
            var block = new Block<string>();
            block.Data = "test";
            block.TimeStamp = new DateTime(2015, 2, 14, 3, 22, 13);
            var hash = dataStore.CalculateHash(block);
            hash.Should().Be("0B97B26F341EA6F768640ABDEE6474EC0495C713BE20FC1A923F1692F076B3E0");

            block = new Block<string>();
            block.Data = "test2";
            block.TimeStamp = new DateTime(2015, 2, 14, 3, 22, 13);
            hash = dataStore.CalculateHash(block);
            hash.Should().Be("22140614524825C3C06A911761CB897A35C97798A9F4263ACC8EE35D34508598");
        }

        [TestMethod]
        public void TryAdd()
        {
            var dataStore = new DataStore<string>();
            var block = new Block<string>();
            block.Data = "test";
            dataStore.TryAdd(block);

            block = new Block<string>()
            {
                Data = "test2block"
            };

            dataStore.TryAdd(block);

            dataStore.ChainLinks.Should().NotBeNullOrEmpty();
        }
    }
}