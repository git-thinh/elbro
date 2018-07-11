using System;
using System.Threading;

namespace elbro
{
    public class JobBase : IJob
    {
        volatile int m_Id = 0;
        volatile JOB_TYPE m_Type = JOB_TYPE.NONE;
        volatile byte m_State = 0; // NONE

        #region [ STATE ]

        bool f_state_PROCESSING_OR_NONE()
        {
             return m_State == 3 || m_State == 0;
        }

        bool f_checkState(JOB_STATE state)
        {
            switch (state)
            {
                case JOB_STATE.NONE:
                    return m_State == 0;
                case JOB_STATE.INIT:
                    return m_State == 1;
                case JOB_STATE.RUNNING:
                    return m_State == 2;
                case JOB_STATE.PROCESSING:
                    return m_State == 3;
                case JOB_STATE.STOPED:
                    return m_State == 4;
            }
            return false;
        }

        protected void f_setState(JOB_STATE state)
        {
            switch (state)
            {
                case JOB_STATE.NONE:
                    m_State = 0;
                    break;
                case JOB_STATE.INIT:
                    m_State = 1;
                    break;
                case JOB_STATE.RUNNING:
                    m_State = 2;
                    break;
                case JOB_STATE.PROCESSING:
                    m_State = 3;
                    break;
                case JOB_STATE.STOPED:
                    m_State = 4;
                    break;
            }
        }

        public JOB_STATE f_getState()
        {
            switch (m_State)
            {
                case 0: return JOB_STATE.NONE;
                case 1: return JOB_STATE.RUNNING;
                case 2: return JOB_STATE.STOPED;
                case 3: return JOB_STATE.INIT;
            }
            return JOB_STATE.NONE;
        }

        #endregion

        public IJobContext JobContext { get; }
        public IJobHandle Handle { get; private set; }
        public JOB_TYPE f_getType() { return m_Type; }

        public int f_getId() { return m_Id; }

        public JobBase(IJobContext jobContext, JOB_TYPE type)
        {
            this.JobContext = jobContext;
            this.m_Id = jobContext.f_getTotalJob() + 1;
            //Interlocked.Add(ref m_Id, jobId);

            f_setState(JOB_STATE.INIT);
            this.m_Type = type;
        }

        public virtual void f_receiveMessage(Message m) { }
        public virtual void f_receiveMessages(Message[] m) { }

        public virtual void f_init() { }
        public virtual void f_processMessage() { }

        public void f_runLoop(object state, bool timedOut)
        {
            if (this.f_state_PROCESSING_OR_NONE()) return;

            if (this.f_checkState(JOB_STATE.STOPED))
            {
                this.f_setState(JOB_STATE.PROCESSING);
                this.Handle.f_actionJobCallback();
                this.f_setState(JOB_STATE.NONE);
                return;
            }

            if (!timedOut)
            {
                f_setState(JOB_STATE.STOPED);
                System.Tracer.WriteLine("J{0} BASE: SIGNAL -> STOP", this.f_getId());
                return;
            }

            switch (this.f_getState())
            {
                case JOB_STATE.INIT:
                    this.f_setState(JOB_STATE.PROCESSING);
                    //System.Tracer.WriteLine("J{0} BASE: SIGNAL -> INITED", this.f_getId());
                    this.Handle = (IJobHandle)state;
                    this.f_init();
                    this.f_setState(JOB_STATE.RUNNING);
                    break;
                case JOB_STATE.RUNNING:
                    this.f_setState(JOB_STATE.PROCESSING);
                    //Tracer.WriteLine("J{0} BASE: Do something ...", this.f_getId());
                    this.f_processMessage();
                    this.f_setState(JOB_STATE.RUNNING);
                    break;
            }
        }


    }
}
