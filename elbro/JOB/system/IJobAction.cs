using System;
using System.Collections.Generic;
using System.Text;

namespace elbro
{
    public interface IJobAction
    {
        void f_eventJobHandleChangeState(JOB_HANDLE_STATE state, int jobId);
        bool f_setData(JOB_TYPE type, string key, object data);
        int f_getTotalJob();
    }
}
