using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace elbro
{
    public interface IJobMonitor
    {
        IJobHandle f_addJobNew(IJob job);
    }

    public class JobMonitor: IJobMonitor
    {
        readonly IJobStore JobStore;
        readonly DictionaryThreadSafe<JOB_TYPE, IJobFactory> JobFactories;
        readonly DictionaryThreadSafe<JOB_TYPE, IJobHandle> JobSingletons;

        public JobMonitor() {
            this.JobStore = new JobStore();
            this.JobFactories = new DictionaryThreadSafe<JOB_TYPE, IJobFactory>();
            this.JobSingletons = new DictionaryThreadSafe<JOB_TYPE, IJobHandle>();
            
            var johMessage = new JobHandle(new JobMessage(this.JobStore), new AutoResetEvent(false));
            this.JobSingletons.Add(JOB_TYPE.MESSAGE, johMessage);
        }

        public IJobHandle f_addJobNew(IJob job)
        {
            return null;
        }
    }
}
