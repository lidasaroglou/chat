using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IMessage
    {
        string Text { get; set; }



    }

    public enum MessageType
    {
        Registration,
        Message
    }
}