using System;
using System.Collections.Generic;
using System.Text;

namespace elbro
{
    public interface IJobAction
    {
        void f_eventJobResponseMessage(int jobId, Message m);
        void f_eventJobHandleChangeState(JOB_HANDLE state, int jobId);

        IMessageContext f_getResponseMessage();

        bool f_setData(JOB_TYPE type, string key, object data);
        int f_getTotalJob();
    }
}
