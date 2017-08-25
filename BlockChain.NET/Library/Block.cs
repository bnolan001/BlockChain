using System;

namespace BlockChain.NET.Library
{
    /// <summary>
    /// Generic object to represent a block of data in the chain
    /// </summary>
    public class Block
    {
        /// <summary>
        /// A reference to the hash of the parent block
        /// </summary>
        public string ParentHash { get; set; }

        /// <summary>
        /// A hash of this block
        /// </summary>
        public string ThisHash { get; set; }

        /// <summary>
        /// The time this block was created(seconds from Unix Epoch)
        /// </summary>
        public UInt32 Timestamp { get; set; }

        /// <summary>
        /// A counter used for the proof-of-work algorithm
        /// </summary>
        public UInt32 Nonce { get; set; }

        /// <summary>
        /// Index of the block with respect to the rest of the chain
        /// </summary>
        public UInt64 Index { get; set; }

        /// <summary>
        /// String version of the data being stored
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Block()
        { }

        /// <summary>
        /// Constructor that initializes core properties
        /// </summary>
        /// <param name="data">String representation of the data</param>
        /// <param name="timestamp">Number of seconds since Unix Epoch that this block was created</param>
        /// <param name="parenthash">Hash value of the parent block</param>
        public Block(string data,
            UInt32 timestamp,
            string parenthash)
            : this()
        {
            Timestamp = timestamp;
            ParentHash = parenthash;
            Data = data;
        }
    }
}