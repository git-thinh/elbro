using System;
using System.Collections.Generic;
using System.Text;

namespace elbro
{
    public interface IJob
    {
        void f_receiveMessage(Message m);
        void f_sendMessage(Message m);
        void f_sendMessages(Message[] ms);
        object f_requestData(object m);

        IJobAction JobAction { get; }

        JOB_STATE f_getState();
        JOB_TYPE f_getType();
        IJobHandle Handle { get; }

        int f_getId();
        int f_getPort();
        bool f_checkKey(object key);
        bool f_setData(string key, object data);
        void f_setId(int id);
        string f_getGroupName();
                        
        void f_runLoop(object state, bool timedOut);
    }
}
