using System;
using System.Threading;

namespace elbro
{
    public class JobMessage : IJob
    {
        readonly QueueThreadSafe<Message> msg;
        readonly ListThreadSafe<oLink> list;
        public IJobStore StoreJob { get; }
        public void f_sendMessage(Message m) { if (this.StoreJob != null) this.StoreJob.f_job_sendMessage(m); }

        private volatile JOB_STATE _state = JOB_STATE.NONE;
        private volatile JOB_TYPE _type = JOB_TYPE.NONE;

        public JOB_STATE f_getState() { return _state; }
        public JOB_TYPE f_getType() { return _type; }

        private volatile int Id = 0;
        public int f_getId() { return Id; }
        public int f_getPort() { return 0; }
        public bool f_checkKey(object key) { return false; }
        public bool f_setData(string key, object data) { return false; }
        public void f_setId(int id)
        {
            //Interlocked.Add(ref Id, id);
            Interlocked.Add(ref Id, id);
        }
        readonly string _groupName = JOB_NAME.SYS_MESSAGE;
        public string f_getGroupName() { return _groupName; }
        public JobMessage(IJobStore _store)
        {
            this.StoreJob = _store;
            list = new ListThreadSafe<oLink>();
            msg = new QueueThreadSafe<Message>();
        }

        //public void f_stopAndFreeResource()
        //{
        //    if (_state != JOB_STATE.STOPED)
        //        lock (jobInfo)
        //            jobInfo.f_stopJob();
        //    list.Clear();
        //    msg.Clear();
        //}

        public void f_receiveMessage(Message m)
        {
            msg.Enqueue(m);
        }

        private volatile bool _inited = false;
        public void f_stopJob()
        {
            list.Clear();
            msg.Clear();

            //jobInfo.f_stopJob();
        }

        private JobHandle jobInfo;
        public void f_runLoop(object state, bool timedOut)
        {
            if (!_inited)
            {
                jobInfo = (JobHandle)state;
                _inited = true;
                
                _state = JOB_STATE.INIT;
                return;
            }
            if (!timedOut)
            {
                System.Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP", Id, Thread.CurrentThread.GetHashCode().ToString());
                // Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP ...", Id, Thread.CurrentThread.GetHashCode().ToString());
                f_stopJob();
                return;
            }

            //if (!_inited)
            //{
            //    _inited = true;
            //    f_Init();
            //    _state = JOB_STATE.INIT;
            //    if (jobInfo != null)
            //        lock (jobInfo)
            //            jobInfo = (JobInfo)state;
            //    else
            //        jobInfo = (JobInfo)state;
            //    return;
            //}

            if (!timedOut)
            {
                //Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP ...", Id, Thread.CurrentThread.GetHashCode().ToString());
                JobHandle ti = (JobHandle)state;
                ti.f_stopJob();
                _state = JOB_STATE.STOPED;
                return;
            }

            if (_state != JOB_STATE.RUNNING) _state = JOB_STATE.RUNNING;

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
    }
}
