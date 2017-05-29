using System;
using System.Runtime.Serialization;

namespace BlockChain.NET.Library
{
    /// <summary>
    /// Generic object to represent a link in the data,
    /// should only be used when Data types already contain
    /// ToString() methods, like StringBuilder, int, double, etc
    /// that convert the value directly to a useable string object
    /// </summary>
    /// <typeparam name="T">Type of the data being stored</typeparam>
    [DataContract]
    public class Block<T> : IBlock<T>
    {
        [DataMember]
        public ulong Index { get; set; }

        [DataMember]
        public string ParentHash { get; set; }

        [DataMember]
        public DateTime TimeStamp { get; set; }

        [DataMember]
        public T Data { get; set; }

        [DataMember]
        public string ThisHash { get; set; }

        public string DataToString()
        {
            return Data?.ToString();
        }
    }
}