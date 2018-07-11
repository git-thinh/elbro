using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace elbro
{
    public class JobMonitor : IJobContext
    {
        readonly IJobHandle MessageHandle;
        readonly DictionaryThreadSafe<JOB_TYPE, IJobFactory> JobFactories = new DictionaryThreadSafe<JOB_TYPE, IJobFactory>();
        readonly DictionaryThreadSafe<JOB_TYPE, IJobHandle> JobSingletons = new DictionaryThreadSafe<JOB_TYPE, IJobHandle>();

        public IMessageContext MessageContext { get; }

        public JobMonitor()
        {
            //var m = new JobMessage();
            //this.MessageContext = m;
            //this.MessageHandle = f_createNew(m);
            //f_createNew(new JobLink(this));
        }

        public IJobHandle f_createNew(IJob job)
        {
            IJobHandle handle = null;
            JOB_TYPE type = job.f_getType();
            switch (job.f_getType())
            {
                case JOB_TYPE.NONE:
                case JOB_TYPE.REQUEST_URL:
                    // factory
                    IJobFactory fac;
                    if (this.JobFactories.ContainsKey(type))
                    {
                        fac = this.JobFactories[type];
                    }
                    else
                    {
                        fac = new JobFactory(type);
                        this.JobFactories.Add(type, fac);
                    }
                    handle = fac.f_createNew(job);
                    break;
                default:
                    // singleton
                    if (!this.JobSingletons.ContainsKey(type))
                    {
                        handle = new JobHandle(job);
                        this.JobSingletons.Add(type, handle);
                    }
                    break;
            }
            return handle;
        }
        
        public int f_getTotalJob()
        {
            IJobFactory[] facs = this.JobFactories.ValuesArray;
            return facs.Sum(x => x.f_count()) + this.JobSingletons.Count;
        }

        public void f_removeAll()
        {
            foreach (var kv in this.JobFactories)
                kv.Value.f_actionJobs(JOB_HANDLE.REMOVE);
            foreach (var kv in this.JobSingletons)
                kv.Value.f_actionJob(JOB_HANDLE.REMOVE);
        }

        public void f_runAll()
        { 
            foreach (var kv in this.JobFactories) 
                    kv.Value.f_actionJobs(JOB_HANDLE.RUN);
            foreach (var kv in this.JobSingletons)
                kv.Value.f_actionJob(JOB_HANDLE.RUN);
        }
        
        public bool f_requestMessages(JOB_TYPE type, Message[] ms,
            Func<IJobHandle, Guid, bool> callBackDoneAll = null)
        {
            //if (this.JobFactories.ContainsKey(type)) {
            //    if (callBackDoneAll != null)
            //        this.HandleMessage.f_getJob().f_setData(JobMessage.REQUEST_MSG_GROUP, 
            //            new Tuple<Func<IRequestMessage, IJobHandle, Guid, bool>, Message[]>(callBackDoneAll, ms));
            //    this.JobFactories[type].f_sendRequestLoadBalancer(ms);
            //} else if (this.JobSingletons.ContainsKey(type)) {
            //    this.JobSingletons[type].f_sendMessages(ms);
            //}
            return false;
        }

        public void f_jobSingletonStateChanged(int jobId, JOB_HANDLE state)
        {
            Tracer.WriteLine("JOB_SINGLETON STATE CHANGED: {0} = {1}", jobId, state);
        }

        ~JobMonitor()
        {
            f_removeAll();
            
            this.JobFactories.Clear();
            this.JobSingletons.Clear();
        }
    }
}
