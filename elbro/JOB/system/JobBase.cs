using System;
using System.Threading;

namespace elbro
{
    public class JobBase : IJob
    {
        volatile int m_Id = 0;
        volatile JOB_TYPE m_Type = JOB_TYPE.NONE;
        volatile byte m_State = 0; // NONE
        volatile IJobHandle m_Handle;

        readonly string m_groupName = string.Empty;

        #region [ STATE ]

        bool f_checkState(JOB_STATE state)
        {
            switch (state)
            {
                case JOB_STATE.NONE:
                    return m_State == 0;
                case JOB_STATE.RUNNING:
                    return m_State == 1;
                case JOB_STATE.STOPED:
                    return m_State == 2;
                case JOB_STATE.INIT:
                    return m_State == 3;
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
                case JOB_STATE.RUNNING:
                    m_State = 1;
                    break;
                case JOB_STATE.STOPED:
                    m_State = 2;
                    break;
                case JOB_STATE.INIT:
                    m_State = 3;
                    break;
            }
        }

        public JOB_STATE f_getState() {
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

        public JOB_TYPE f_getType() { return m_Type; }

        public IJobAction JobAction { get; }

        public int f_getId() { return m_Id; }
        public string f_getGroupName() { return m_groupName; }

        public void f_setId(int id) { Interlocked.Add(ref m_Id, id); }

        public JobBase(JOB_TYPE type, IJobAction jobAction)
        {
            f_setState(JOB_STATE.INIT);
            this.m_Type = type;
            this.JobAction = jobAction;
        }
         
        public virtual int f_getPort() { return 0; }
        public virtual bool f_checkKey(object key) { return false; }
        public virtual bool f_setData(string key, object data) { return false; }

        public virtual void f_sendMessage(Message m) { }
        public virtual void f_receiveMessage(Message m) { }

        public virtual void f_Init() { }
        public virtual void f_processMessage() { }

        public void f_runLoop(object state, bool timedOut)
        {
            if (this.f_checkState(JOB_STATE.STOPED)) return;

            if (!timedOut)
                f_setState(JOB_STATE.STOPED);

            switch (this.f_getState())
            {
                case JOB_STATE.INIT:
                    //System.Tracer.WriteLine("J{0} BASE: SIGNAL -> INITED", this.f_getId());
                    this.m_Handle = (IJobHandle)state;
                    this.f_setState(JOB_STATE.RUNNING);
                    this.f_Init();
                    break;
                case JOB_STATE.STOPED:
                    //System.Tracer.WriteLine("J{0} BASE: SIGNAL -> STOP", this.f_getId());
                    this.m_Handle.f_eventJobStoped();
                    break;
                case JOB_STATE.RUNNING:
                    //Tracer.WriteLine("J{0} BASE: Do something ...", this.f_getId());
                    this.f_processMessage();
                    break;
            }
        }
    }
}
