using System;
using System.Runtime.Serialization;

namespace BlockChain.NET.Library
{
    public interface IBlock<T>
    {
        [DataMember]
        ulong Index { get; set; }

        [DataMember]
        string ParentHash { get; set; }

        [DataMember]
        DateTime TimeStamp { get; set; }

        [DataMember]
        T Data { get; set; }

        [DataMember]
        string ThisHash { get; set; }

        string DataToString();
    }
}