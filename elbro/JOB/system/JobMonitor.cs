using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace elbro
{
    public interface IJobMonitor
    {
        IJobHandle f_createNew(IJob job);
        void f_removeAll();
        void f_runAll();
    }

    public class JobMonitor: IJobMonitor, IJobAction
    {
        readonly DictionaryThreadSafe<JOB_TYPE, IJobFactory> JobFactories;
        readonly DictionaryThreadSafe<JOB_TYPE, IJobHandle> JobSingletons;

        public JobMonitor() {
            this.JobFactories = new DictionaryThreadSafe<JOB_TYPE, IJobFactory>();
            this.JobSingletons = new DictionaryThreadSafe<JOB_TYPE, IJobHandle>();

            //f_createNew(new JobMessage(this));
            //f_createNew(new JobLink(this));
        }

        public IJobHandle f_createNew(IJob job)
        {
            IJobHandle handle = null;
            JOB_TYPE type = job.f_getType();
            switch (job.f_getType()) {
                case JOB_TYPE.NONE:
                case JOB_TYPE.REQUEST_URL: 
                    // factory
                    IJobFactory fac;
                    if (this.JobFactories.ContainsKey(type)) {
                        fac = this.JobFactories[type];
                    } else {
                        fac = new JobFactory(type);
                        this.JobFactories.Add(type, fac);
                    }
                    handle = fac.f_createNew(job);
                    handle.f_runJob();
                    break;
                default: 
                    // singleton
                    if (!this.JobSingletons.ContainsKey(type))
                    {
                        handle = new JobHandle(job, new AutoResetEvent(false));
                        this.JobSingletons.Add(type, handle);
                        handle.f_runJob();
                    }
                    break;
            }
            return handle;
        }

        public int f_getTotalJob()
        {
            return this.JobFactories.Values.Sum(x => x.f_count()) + this.JobSingletons.Count;
        }

        public void f_removeAll()
        {
            foreach (var kv in this.JobFactories)
                kv.Value.f_removeJobs();
            foreach (var kv in this.JobSingletons)
                kv.Value.f_removeJob();
        }

        public void f_runAll()
        {
            foreach (var kv in this.JobFactories)
                kv.Value.f_runJobs();
            foreach (var kv in this.JobSingletons)
                kv.Value.f_runJob();
        }

        public bool f_setData(JOB_TYPE type, string key, object data)
        {
            return false;
        }
    }
}
