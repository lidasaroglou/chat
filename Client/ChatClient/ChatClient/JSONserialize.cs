using Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient 
{
    class JSONserialize : IMessage
    {
        public string Receiver
        {
            get;
            set;
           
        }

        public string Text
        {
            get;
            set;
        }

        public MessageType Type
        {
            get;
            set;
            
        }
    }
}
