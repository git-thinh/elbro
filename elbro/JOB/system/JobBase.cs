using System;
using System.Threading;

namespace elbro
{
    public class JobBase : IJob
    {
        volatile int m_id = 0;
        volatile JOB_TYPE m_type = JOB_TYPE.NONE;

        protected volatile JOB_STATE m_state = JOB_STATE.NONE;
        protected volatile bool m_inited = false;
        protected volatile IJobHandle m_jobHandle;

        readonly string m_groupName = string.Empty;


        public JOB_STATE f_getState() { return m_state; }
        public JOB_TYPE f_getType() { return m_type; }

        public IJobAction JobAction { get; }
        
        public int f_getId() { return m_id; }
        public string f_getGroupName() { return m_groupName; }

        public void f_setId(int id) { Interlocked.Add(ref m_id, id); }
        
        public JobBase(JOB_TYPE type, IJobAction jobAction)
        {
            this.m_state = JOB_STATE.INIT;
            this.m_type = type;
            this.JobAction = jobAction;
        }

        public void f_stopJob()
        {
            m_jobHandle.f_stopJob();
        }
        
        public virtual int f_getPort() { return 0; }
        public virtual bool f_checkKey(object key) { return false; }
        public virtual bool f_setData(string key, object data) { return false; }

        public virtual void f_sendMessage(Message m) { }

        public virtual void f_receiveMessage(Message m) { }

        public virtual void f_runLoop(object state, bool timedOut)
        {
            //if (!m_inited)
            //{
            //    m_jobHandle = (IJobHandle)state;
            //    m_inited = true;
            //    return;
            //}
            //if (!timedOut)
            //{
            //    System.Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP", m_id, Thread.CurrentThread.GetHashCode().ToString());
            //    // Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP ...", Id, Thread.CurrentThread.GetHashCode().ToString());
            //    f_stopJob();
            //    return;
            //}

            ////JobInfo ti = (JobInfo)state;
            ////if (!timedOut)
            ////{
            ////    Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP", Id, Thread.CurrentThread.GetHashCode().ToString());
            ////    ti.f_stopJob();
            ////    return;
            ////}

            //Tracer.WriteLine("J{0} executes on thread {1}: Do something ...", m_id, Thread.CurrentThread.GetHashCode().ToString());
        } 
    }
}
