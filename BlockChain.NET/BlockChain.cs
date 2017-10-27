using BlockChain.NET.Library;
using System;

namespace BlockChain.NET
{
    public class BlockChain
    {
        private DataStore _datStore;
        private int _intNumLeadingZeroes;

        /// <summary>
        /// Default contructor
        /// </summary>
        public BlockChain(int numLeadingZeroes)
        {
            _intNumLeadingZeroes = numLeadingZeroes;
            _datStore = new DataStore("",
                0,
                "0000000000000000000000000000000",
                _intNumLeadingZeroes);
        }

        public void UpdateDifficulty(int numLeadingZeroes)
        {
            _intNumLeadingZeroes = numLeadingZeroes;
        }

        /// <summary>
        /// Takes the Data from the block object, creates a new link block
        /// and adds it to the chain
        /// </summary>
        /// <param name="block">New block to add to the chain</param>
        /// <returns>False if the block could not be added due to a hash collision</returns>
        public bool TryAdd(Block block)
        {
            return _datStore.TryAdd(block,
                _intNumLeadingZeroes);
        }

        /// <summary>
        /// Takes the Data, creates a new link block and adds it to the chain
        /// </summary>
        /// <param name="data">String representation of the data</param>
        /// <param name="timestamp">Number of seconds since Unix Epoch that this block was created</param>
        /// <param name="parenthash">Hash value of the parent block</param>
        /// <returns>False if the block could not be added due to a hash collision</returns>
        public bool TryAdd(string data, UInt32 timestamp, string parenthash)
        {
            return _datStore.TryAdd(data, timestamp, parenthash,
                _intNumLeadingZeroes);
        }

        /// <summary>
        /// Retrieves the block associated with the hash value
        /// </summary>
        /// <param name="blockHash">Unique hash of the block to find</param>
        /// <param name="link">Copy of the block object</param>
        /// <returns>False if it could not find the block hash</returns>
        public bool TryGet(string blockHash, out Block link)
        {
            return _datStore.TryGet(blockHash, out link);
        }
    }
}