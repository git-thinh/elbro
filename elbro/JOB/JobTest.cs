using System;
using System.Threading;

namespace elbro
{
    public class JobTest : IJob
    {
        private volatile JOB_STATE _state = JOB_STATE.NONE;
        public JOB_STATE State { get { return _state; } }
        public IJobStore StoreJob { get; }
        public void f_stopAndFreeResource() { }
        public void f_sendMessage(Message m) { if (this.StoreJob != null) this.StoreJob.f_job_sendMessage(m); }
        public void f_receiveMessage(Message m) { }

        private volatile int Id = 0;
        public int f_getId() { return Id; }
        public int f_getPort() { return 0; }
        public bool f_checkKey(object key) { return false; }
        public bool f_setData(string key, object data) { return false; }
        public void f_setId(int id) { Interlocked.Add(ref Id, id); }
        readonly string _groupName = string.Empty;
        public string f_getGroupName() { return _groupName; }


        public JobTest(IJobStore _store)
        {
            this.StoreJob = _store;
        }


        private volatile bool _inited = false;
        public void f_stopJob()
        {
            jobInfo.f_stopJob();
        }

        private JobHandle jobInfo;
        public void f_runLoop(object state, bool timedOut)
        {
            if (!_inited)
            {
                jobInfo = (JobHandle)state;
                _inited = true;
                return;
            }
            if (!timedOut)
            {
                System.Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP", Id, Thread.CurrentThread.GetHashCode().ToString());
                // Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP ...", Id, Thread.CurrentThread.GetHashCode().ToString());
                f_stopJob();
                return;
            }

            //JobInfo ti = (JobInfo)state;
            //if (!timedOut)
            //{
            //    Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP", Id, Thread.CurrentThread.GetHashCode().ToString());
            //    ti.f_stopJob();
            //    return;
            //}

            Tracer.WriteLine("J{0} executes on thread {1}: Do something ...", Id, Thread.CurrentThread.GetHashCode().ToString());
        }

        
    }
}
