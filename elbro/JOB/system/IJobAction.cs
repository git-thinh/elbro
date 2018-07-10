using System;
using System.Collections.Generic;
using System.Text;

namespace elbro
{
    public interface IJobAction
    {
        bool f_setData(JOB_TYPE type, string key, object data);
        int f_getTotalJob();
    }
}
