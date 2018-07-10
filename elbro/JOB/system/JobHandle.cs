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
        void f_eventJobStoped();
        void f_resetJob();
        void f_pauseJob();
        void f_removeJob();
        
        IJob f_getJob();
        JOB_HANDLE_STATE f_getState();

        void f_receiveMessage(Message m);
        void f_sendMessage(Message m);
        void f_sendMessages(Message[] ms);
    }

    public enum JOB_HANDLE_STATE {
        NONE,
        PAUSE,
        STOP,
        RESET,
        REMOVE, 
        CLEAR,
        RUN,
    }

    public class JobHandle: IJobHandle
    {
        readonly IJob Job;
        readonly AutoResetEvent EvenStopLoop;
        readonly static Random _random = new Random(); 
        RegisteredWaitHandle Handle;
        JOB_HANDLE_STATE HandleCurrent;

        public JobHandle(IJob job, AutoResetEvent ev)
        {
            this.HandleCurrent = JOB_HANDLE_STATE.NONE;
            int id = job.JobAction.f_getTotalJob() + 1;
            job.f_setId(id); 

            this.Job = job;
            this.EvenStopLoop = ev;
        }
        
        public JOB_HANDLE_STATE f_getState()
        {
            return this.HandleCurrent;
        }

        public void f_runJob()
        {
            this.HandleCurrent = JOB_HANDLE_STATE.RUN;
            this.Handle = ThreadPool.RegisterWaitForSingleObject(
                this.EvenStopLoop,
                new WaitOrTimerCallback(this.Job.f_runLoop),
                this,
                JOB_CONST.JOB_TIMEOUT_RUN,
                false);
        }

        public void f_resetJob()
        { 
            if (this.Handle != null)
                this.Handle.Unregister(null);

            this.EvenStopLoop.Reset();

            this.Handle = ThreadPool.RegisterWaitForSingleObject(
                this.EvenStopLoop,
                new WaitOrTimerCallback(this.Job.f_runLoop),
                this,
                JOB_CONST.JOB_TIMEOUT_RUN,
                false);
        }

        void f_postEventStopLoop(JOB_HANDLE_STATE cmd)
        {
            if (this.EvenStopLoop != null)
            {
                this.HandleCurrent = cmd;
                if (this.EvenStopLoop != null)
                    this.EvenStopLoop.Set();
            }
        }

        public void f_eventJobStoped()
        {
            switch (this.HandleCurrent) {
                case JOB_HANDLE_STATE.PAUSE:
                    break;
                case JOB_HANDLE_STATE.STOP:
                    this.f_getJob().JobAction.f_eventJobHandleChangeState(this.HandleCurrent, this.Job.f_getId());
                    break;
                case JOB_HANDLE_STATE.RESET:
                    break;
                case JOB_HANDLE_STATE.REMOVE:
                    if (this.Handle != null)
                    {
                        this.Handle.Unregister(null);
                        this.Handle = null;
                    }
                    this.EvenStopLoop.Close();
                    this.f_getJob().JobAction.f_eventJobHandleChangeState(this.HandleCurrent, this.Job.f_getId());
                    break;
            }
        }
        
        public void f_stopJob()
        { 
            f_postEventStopLoop(JOB_HANDLE_STATE.STOP);
        }

        public void f_pauseJob()
        {
            f_postEventStopLoop(JOB_HANDLE_STATE.PAUSE);
        }

        public void f_removeJob()
        {
            f_postEventStopLoop(JOB_HANDLE_STATE.REMOVE);             
        }
        
        public IJob f_getJob() {
            return this.Job;
        }


        public void f_receiveMessage(Message m)
        {
            if (Job != null)
                Job.f_receiveMessage(m);
        }

        public void f_sendMessage(Message m)
        {
            //if (this.Job.f_getGroupName() == JOB_NAME.SYS_MESSAGE)
            //    f_receiveMessage(m);
            //else
            //{
            //    if (this.Job.StoreJob != null)
            //        this.Job.StoreJob.f_job_sendMessage(m);
            //}
            this.Job.f_sendMessage(m);
        }

        public void f_sendMessages(Message[] ms)
        {
            this.Job.f_sendMessages(ms);
        }




        public AutoResetEvent f_getEvent() { return EvenStopLoop; }
        
        public string f_getGroupName() { return this.Job.f_getGroupName(); }


        public override string ToString() { return this.Job.f_getId().ToString(); }

    }
}
