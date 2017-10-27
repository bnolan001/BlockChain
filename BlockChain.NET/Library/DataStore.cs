using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BlockChain.NET.Library
{
    public class DataStore
    {
        private List<Block> _lisChainLinks;
        private Dictionary<string, Block> _dicChainLinksByBlockHash;

        /// <summary>
        /// Returns a readonly version of the chain
        /// </summary>
        public ReadOnlyCollection<Block> ChainLinks
        {
            get { return _lisChainLinks.AsReadOnly(); }
        }

        /// <summary>
        /// Constructor which initializes the chain with a base block object
        /// </summary>
        /// <param name="data">String representation of the data</param>
        /// <param name="timestamp">Number of seconds since Unix Epoch that this block was created</param>
        /// <param name="parenthash">Hash value of the parent block</param>
        public DataStore(string data,
            UInt32 timestamp,
            string parenthash,
            int numLeadingZeros)
        {
            _dicChainLinksByBlockHash = new Dictionary<string, Block>();
            _lisChainLinks = new List<Block>();
            TryAdd(data, timestamp, parenthash, numLeadingZeros);
        }

        /// <summary>
        /// Takes the Data from the block object, creates a new link block
        /// and adds it to the chain
        /// </summary>
        /// <param name="block">New block to add to the chain</param>
        /// <returns>False if the block could not be added due to a hash collision</returns>
        public bool TryAdd(Block block,
            int numLeadingZeros)
        {
            bool success = false;
            int linkCount = _lisChainLinks.Count;
            ulong nextIndex = linkCount == 0 ?
                0 : _lisChainLinks[linkCount - 1].Index + 1;

            // Deep copy the object and set all fields
            var link = ObjectExtensions.Copy(block);
            link.Index = nextIndex;
            CalculateHash(link, numLeadingZeros);

            // If the hash already exists then don't add it
            if (!_dicChainLinksByBlockHash.ContainsKey(link.ThisHash))
            {
                _lisChainLinks.Add(link);
                _dicChainLinksByBlockHash[link.ThisHash] =
                    _lisChainLinks[linkCount];
                success = true;
            }
            else
            {
                // Should create custom exception
                throw new ArgumentException("Block Hash already exists in the Chain");
            }

            return success;
        }

        /// <summary>
        /// Takes the Data, creates a new link block and adds it to the chain
        /// </summary>
        /// <param name="data">String representation of the data</param>
        /// <param name="timestamp">Number of seconds since Unix Epoch that this block was created</param>
        /// <param name="parenthash">Hash value of the parent block</param>
        /// <returns>False if the block could not be added due to a hash collision</returns>
        public bool TryAdd(string data,
            UInt32 timestamp,
            string parenthash,
            int numLeadingZeros)
        {
            var block = new Block()
            {
                Timestamp = timestamp,
                ParentHash = parenthash,
                Data = data
            };

            return TryAdd(block, numLeadingZeros);
        }

        /// <summary>
        /// Retrieves the block associated with the hash value
        /// </summary>
        /// <param name="blockHash">Unique hash of the block to find</param>
        /// <param name="link">Copy of the block object</param>
        /// <returns>False if it could not find the block hash</returns>
        public bool TryGet(string blockHash, out Block link)
        {
            bool success = false;
            link = null;

            if (_dicChainLinksByBlockHash.ContainsKey(blockHash))
            {
                // Deep copy the object
                link = ObjectExtensions.Copy(_dicChainLinksByBlockHash[blockHash]);
            }
            return success;
        }

        /// <summary>
        /// Calculates the SHA 256bit hash of the block
        /// </summary>
        /// <param name="block">Object to generate a hash of</param>
        /// <returns>String representation of the SHA256 bit hash for the block</returns>
        public void CalculateHash(Block block,
            int numLeadingZeros)
        {
            StringBuilder sbZeroes = new StringBuilder("");
            for (int ct = 0; ct < numLeadingZeros; ct++)
            {
                sbZeroes.Append("0");
            }
            var zeroes = sbZeroes.ToString();
            uint nonce = 0;
            var hashString = string.Empty;

            do
            {
                block.Nonce = nonce;
                var json = JsonConvert.SerializeObject(block);
                byte[] hash = null;
                // Initialize a SHA256 hash object.
                SHA256 mySHA256 = SHA256Managed.Create();
                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    // Compute the hash of the fileStream.
                    hash = mySHA256.ComputeHash(memoryStream);
                }
                hashString = ByteArrayToString(hash);
                nonce++;
            } while (!hashString.StartsWith(zeroes));

            block.ThisHash = hashString;
        }

        /// <summary>
        /// Converts a byte array to a hexadecimal string
        /// </summary>
        /// <param name="byteArray">Non-null array of bytes</param>
        /// <returns>String representation of the array in hexadecimal</returns>
        public string ByteArrayToString(byte[] byteArray)
        {
            StringBuilder sbHex = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
                sbHex.AppendFormat("{0:x2}", b);
            return sbHex.ToString();
        }
    }
}