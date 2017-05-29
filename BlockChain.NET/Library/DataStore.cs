using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;

namespace BlockChain.NET.Library
{
    public class DataStore<T>
    {
        private List<IBlock<T>> _lisChainLinks;
        private Dictionary<string, IBlock<T>> _dicChainLinksByBlockHash;

        /// <summary>
        /// Returns a readonly version of the chain
        /// </summary>
        public ReadOnlyCollection<IBlock<T>> ChainLinks
        {
            get { return _lisChainLinks.AsReadOnly(); }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataStore()
        {
            _lisChainLinks = new List<IBlock<T>>();
            _dicChainLinksByBlockHash = new Dictionary<string, IBlock<T>>();
        }

        /// <summary>
        /// Takes the Data from the block object, creates a new link block
        /// and adds it to the chain
        /// </summary>
        /// <param name="block"></param>
        /// <returns>False if the block could not be added due to a hash collision</returns>
        public bool TryAdd(IBlock<T> block)
        {
            bool success = false;
            int linkCount = _lisChainLinks.Count;
            ulong nextIndex = linkCount == 0 ?
                0 : _lisChainLinks[linkCount - 1].Index + 1;
            string parentHash = linkCount == 0 ?
                "0000000000000000000000000000000000000000000000000000000000000000" :
                _lisChainLinks[linkCount - 1].ThisHash;

            // Deep copy the object and set all fields
            var link = ObjectExtensions.Copy(block);
            link.Index = nextIndex;
            link.ParentHash = parentHash;
            link.TimeStamp = DateTime.UtcNow;
            link.ThisHash = CalculateHash(link);

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
        /// <param name="data"></param>
        /// <returns></returns>
        public bool TryAdd(T data)
        {
            var block = new Block<T>()
            {
                Data = data
            };

            return TryAdd(block);
        }

        /// <summary>
        /// Retrieves the block associated with the hash value
        /// </summary>
        /// <param name="blockHash"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public bool TryGet(string blockHash, out IBlock<T> link)
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
        /// <param name="block"></param>
        /// <returns></returns>
        public string CalculateHash(IBlock<T> block)
        {
            var json = JsonConvert.SerializeObject(block);
            byte[] hash = null;
            // Initialize a SHA256 hash object.
            SHA256 mySHA256 = SHA256Managed.Create();
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                // Compute the hash of the fileStream.
                hash = mySHA256.ComputeHash(memoryStream);
            }
            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}