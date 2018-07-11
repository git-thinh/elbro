using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace elbro
{
    public delegate void EventReceiveMessage(IFORM form, Message m);
    public interface IFORM
    {
        int f_getFormID();

        void f_receiveMessage(Guid id);
        event EventReceiveMessage OnReceiveMessage;
    }
}
