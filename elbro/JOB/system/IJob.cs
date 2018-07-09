using System;
using System.Collections.Generic;
using System.Text;

namespace elbro
{
    public interface IJob
    {
        void f_receiveMessage(Message m);
        void f_sendMessage(Message m);

        IJobStore StoreJob { get; }

        JOB_STATE f_getState();
        JOB_TYPE f_getType();

        void f_stopJob();

        int f_getId();
        int f_getPort();
        bool f_checkKey(object key);
        bool f_setData(string key, object data);
        void f_setId(int id);
        string f_getGroupName();
                        
        void f_runLoop(object state, bool timedOut);
    }
}
