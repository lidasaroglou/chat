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

        MessageType Type { get; set; }

        string Receiver { get; set; }
    }

    /*
     * 
     * { 'Text' : 'Geia', 'Type' : 'Broadcast' }
     */

    public enum MessageType
    {
        Registration,
        Broadcast,
        Unicast
    }
}