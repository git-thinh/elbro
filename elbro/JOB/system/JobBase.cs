using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace elbro
{
    public class JobBase : IJob
    {
        volatile int m_Id = 0; 
        volatile byte Status = 0; /* 0: none */

        public JOB_STATE State {
            get {
                switch (this.Status) {
                    case 1: return JOB_STATE.INIT;
                    case 2: return JOB_STATE.RUNNING;
                    case 3: return JOB_STATE.PROCESSING;
                    case 4: return JOB_STATE.STOPED;
                }
                return JOB_STATE.NONE;
            }
        }
        public IJobContext JobContext { get; }
        public IJobHandle Handle { get; private set; }
        public JOB_TYPE Type { get; }

        public int f_getId() { return m_Id; }

        public JobBase(IJobContext jobContext, JOB_TYPE type)
        {
            this.JobContext = jobContext;
            this.m_Id = jobContext.f_getTotalJob() + 1;
            this.Type = type;
            this.Status = 1; /* 1: init */
        }

        public virtual void f_receiveMessage(Message m) { }
        public virtual void f_receiveMessages(Message[] m) { }

        public virtual void f_init() { }
        public virtual Guid f_processMessage(Message m) { return Guid.Empty; }
        
        delegate Guid ProcessMessage(Message m);
        void f_callbackProcessMessage(IAsyncResult asyncRes) {
            AsyncResult ares = (AsyncResult)asyncRes;
            ProcessMessage delg = (ProcessMessage)ares.AsyncDelegate;
            Guid result = delg.EndInvoke(asyncRes);

            Thread.Sleep(3000);

            f_runLoop(this.Handle);
        }

        public void f_runLoop(IJobHandle handle)
        {            
            //if (f_lockCheck())
            //    return;
            //f_lock();
            
            //if (!timedOut)
            //{
            //    System.Tracer.WriteLine("J{0} BASE: STOP ...", this.f_getId());
            //    this.Handle.f_actionJobCallback();

            //    f_state_setStop();
            //    // do not unlock until call f_Restart();
            //    return;
            //}

            /* 1: init */
            if (this.Status == 1)  
            {
                System.Tracer.WriteLine("J{0} BASE: INITED ...", this.f_getId());
                this.Handle = handle;
                this.f_init();

                this.Status = 2; /* 2: running */

                f_runLoop(handle);
            }

            /* 2: running */
            if (this.Status == 2) {
                Tracer.WriteLine("J{0} BASE: RUNNING ...", this.f_getId());
                ProcessMessage fun = this.f_processMessage;
                Message m = new Message();
                IAsyncResult asyncRes = fun.BeginInvoke(m, new AsyncCallback(f_callbackProcessMessage), null);

                ///// check timeout ...
                //IAsyncResult asyncRes = fun.BeginInvoke(m, null, null);
                //// Poll IAsyncResult.IsCompleted
                //while (asyncRes.IsCompleted == false)
                //{
                //    Console.WriteLine("Square Number still processing");
                //    Thread.Sleep(1000);  // emulate that method is busy
                //}
                //Console.WriteLine("Square Number processing completed");
                //Guid res = fun.EndInvoke(asyncRes);

            }
            
            /// end function
            ///////////////////////
        }
    }
}
