using System;
using System.Threading;

namespace elbro
{
    public class JobMessage : JobBase
    {
        readonly QueueThreadSafe<Message> msg;

        public JobMessage(IJobStore store) : base(JOB_TYPE.MESSAGE, store)
        {
            msg = new QueueThreadSafe<Message>();
        }
        
        public override void f_receiveMessage(Message m)
        {
            msg.Enqueue(m);
        }

        public override void f_runLoop(object state, bool timedOut)
        {
            if (!this.m_inited)
            {
                this.m_jobHandle  = (IJobHandle)state;
                this.m_inited = true;                
                this.m_state = JOB_STATE.RUNNING;

                return;
            }
            if (!timedOut)
            {
                System.Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP", this.f_getId(), Thread.CurrentThread.GetHashCode().ToString());
                // Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP ...", Id, Thread.CurrentThread.GetHashCode().ToString());
                f_stopJob();
                return;
            }
             
            //Tracer.WriteLine("J{0} executes on thread {1}:DO SOMETHING ...", Id, Thread.CurrentThread.GetHashCode().ToString());
            // Do something ...

            if (msg.Count > 0)
            {
                Message m = msg.Dequeue(null);
                if (m != null)
                {
                    //[1] SEND REQUEST TO JOB FOR EXECUTE
                    if (m.Type == MESSAGE_TYPE.REQUEST)
                    {
                        IJob[] jobs = this.StoreJob.f_job_getByID(m.GetReceiverId());
                        if (jobs.Length > 0)
                            for (int i = 0; i < jobs.Length; i++)
                                jobs[i].f_receiveMessage(m);
                    }
                    else
                    {
                        //[2] RESPONSE TO SENDER
                        switch (m.getSenderType())
                        {
                            case SENDER_TYPE.IS_FORM:
                                IFORM fom = this.StoreJob.f_form_Get(m.GetSenderId());
                                if (fom != null)
                                    fom.f_receiveMessage(m.GetMessageId());
                                // write to LOG ...
                                break;
                            case SENDER_TYPE.HIDE_SENDER:
                                // do not send response to sender
                                // write to LOG ...
                                break;
                        }
                    }
                }
            }
        }

        ~JobMessage() {
            msg.Clear();
        }
    }
}
