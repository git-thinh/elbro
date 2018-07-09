using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace elbro
{
    public interface IJobHandle
    {
        void f_runJob();
        void f_stopJob();
        void f_resetJob();
        void f_pauseJob();
        void f_removeJob();
        
        IJob f_getJob();

        void f_receiveMessage(Message m);
        void f_sendMessage(Message m);
    }

    public class JobHandle: IJobHandle
    {
        readonly IJob Job;
        readonly AutoResetEvent Even;
        readonly static Random _random = new Random(); 
        RegisteredWaitHandle Handle;

        public JobHandle(IJob job, AutoResetEvent ev)
        {
            job.f_setId(job.StoreJob.f_job_countAll() + 1);

            this.Job = job;
            this.Even = ev;            
        }


        #region [ Job Handle ]

        public void f_runJob()
        {
            this.Handle = ThreadPool.RegisterWaitForSingleObject(
                this.Even,
                new WaitOrTimerCallback(this.Job.f_runLoop),
                this,
                JOB_CONST.JOB_TIMEOUT_RUN,
                false);
        }

        public void f_resetJob()
        { 
            if (this.Handle != null)
                this.Handle.Unregister(null);

            this.Even.Reset();

            this.Handle = ThreadPool.RegisterWaitForSingleObject(
                this.Even,
                new WaitOrTimerCallback(this.Job.f_runLoop),
                this,
                JOB_CONST.JOB_TIMEOUT_RUN,
                false);
        }

        public void f_stopJob() {
            if (this.Job != null)
                this.Job.f_stopJob();
        }

        public void f_pauseJob()
        {
        }

        public void f_removeJob()
        { 
            if (this.Handle != null)
                this.Handle.Unregister(null);
            this.Job.StoreJob.f_job_eventAfterStop(this.Job.f_getId());
        }
        
        public IJob f_getJob() {
            return this.Job;
        }


        public void f_receiveMessage(Message m)
        {
            if (Job != null)
                Job.f_receiveMessage(m);
        }
         
        public void f_sendMessage(Message m) {
            if (this.Job.f_getGroupName() == JOB_NAME.SYS_MESSAGE)
                f_receiveMessage(m);
            else
            {
                if (this.Job.StoreJob != null)
                    this.Job.StoreJob.f_job_sendMessage(m);
            }
        }
         
        #endregion




        public AutoResetEvent f_getEvent() { return Even; }
        
        public string f_getGroupName() { return this.Job.f_getGroupName(); }


        public override string ToString() { return this.Job.f_getId().ToString(); }
    }
}
