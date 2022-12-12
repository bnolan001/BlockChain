using BlockChain.NET.Library;
using FluentAssertions;
using Newtonsoft.Json;

namespace Testing.Unit
{
    [TestFixture]
    public class DataStoreTesting
    {
        [Test]
        public void CalculateHash()
        {
            var dataStore = new DataStore("",
                0,
                "0000000000000000000000000000000",
                0);
            var block = new Block();
            block.Data = "test";
            TimeSpan ts = new DateTimeOffset(2015, 2, 14, 3, 22, 13, TimeSpan.Zero)
                .Subtract(new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            dataStore.CalculateHash(block,
                0);
            block.ThisHash.Should().Be("b159409daaab04d3669973a61bf98715c6975c913321c41a8fb2c76a54121d5a");

            block = new Block();
            block.Data = "test2";
            ts = new DateTimeOffset(2015, 2, 14, 3, 22, 13, TimeSpan.Zero)
                .Subtract(new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            dataStore.CalculateHash(block,
                0);
            block.ThisHash.Should().Be("7bec0d90805220d228fa51bf9f0907a69088c6246a003f3fa66a92fa32128278");

            block = new Block();
            block.Data = "test";
            ts = new DateTimeOffset(2015, 2, 14, 3, 22, 13, TimeSpan.Zero)
                .Subtract(new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            dataStore.CalculateHash(block,
                4);
            block.ThisHash.Should().Be("000068a4fb51099a1f6132557f3ee053770afba130e7728cae82552815fb473e");
            block.Nonce.Should().Be(43300);

            block = new Block();
            block.Data = JsonConvert.SerializeObject(new
            {
                Transactions = new[]
                {
                    new {
                        Sender = "a0d4efb34",
                        Receiver = "de345acf2",
                        Currency = "IOTA",
                        Amount = 39201456.3900012,
                        Date = "2017-07-22T18:32:28.8976Z"
                    },
                    new {
                        Sender = "ef38717a76",
                        Receiver = "a00d938fcd",
                        Currency = "Ethereum",
                        Amount = 2.03,
                        Date = "2017-07-22T18:32:29.1094Z"
                    }
                }
            });
            ts = new DateTimeOffset(2017, 7, 22, 18, 32, 30, TimeSpan.Zero)
                .Subtract(new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero));
            block.Timestamp = Convert.ToUInt32(ts.TotalSeconds);
            dataStore.CalculateHash(block,
                4);
            block.ThisHash.Should().Be("00004be94f3fb992314c75e6a9e6a9d49650fee9163082c6aa1b51c04f990191");
            block.Nonce.Should().Be(21254);
        }

        [Test]
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

            dataStore.ChainLinks[0].Data.Should().Be("");
            dataStore.ChainLinks[1].Data.Should().Be("test");
            dataStore.ChainLinks[2].Data.Should().Be("test2block");
        }

        [Test]
        public void VerifyRreadOnly()
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

            // Try to set the data values
            dataStore.ChainLinks[0].Data = "invalid";
            dataStore.ChainLinks[1].Data = "not editable";
            dataStore.ChainLinks[2].Data = "leave it alone";
            // Verify the values are still the original ones
            dataStore.ChainLinks[0].Data.Should().Be("");
            dataStore.ChainLinks[1].Data.Should().Be("test");
            dataStore.ChainLinks[2].Data.Should().Be("test2block");
        }
    }
}