using System;
using System.Collections.Generic;
using System.Text;

namespace elbro
{
    public interface IJob
    {
        JOB_STATE State { get; }
        IJobStore StoreJob { get; }

        void f_stopJob();

        int f_getId();
        int f_getPort();
        bool f_checkKey(object key);
        void f_setId(int id);
        string f_getGroupName();

        void f_receiveMessage(Message m);
        void f_sendMessage(Message m);
                
        void f_runLoop(object state, bool timedOut);
    }
}
