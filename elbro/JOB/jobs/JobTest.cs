using System;
using System.Threading;

namespace elbro
{
    public class JobTest : JobBase
    {
        public JobTest(IJobAction jobAction) : base(JOB_TYPE.NONE, jobAction)
        {
        } 

        public override void f_sendMessage(Message m)
        {
        }
        public override void f_receiveMessage(Message m)
        {
        }

        public override int f_getPort()
        {
            return 0;
        }
        public override bool f_checkKey(object key)
        {
            return false;
        }
        public override bool f_setData(string key, object data)
        {
            return false;
        }
        
        public override void f_runLoop(object state, bool timedOut)
        {
            if (!this.m_inited)
            {
                this.m_jobHandle = (IJobHandle)state;
                this.m_inited = true;
                return;
            }
            if (!timedOut)
            {
                System.Tracer.WriteLine("J{0}: SIGNAL -> STOP", this.f_getId());
                //f_stopJob();
                return;
            }

            //JobInfo ti = (JobInfo)state;
            //if (!timedOut)
            //{
            //    Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP", Id, Thread.CurrentThread.GetHashCode().ToString());
            //    ti.f_stopJob();
            //    return;
            //}

            Tracer.WriteLine("J{0}: Do something ...", this.f_getId());
        } 
    }
}
