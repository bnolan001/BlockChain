using BlockChain.NET.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.NET
{
    public class BlockChain<T>
    {
        private DataStore<T> _datStore;

        public BlockChain()
        {
            _datStore = new DataStore<T>();
        }
    }
}