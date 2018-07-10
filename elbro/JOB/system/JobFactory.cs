using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace elbro
{
    public interface IJobFactory
    {
        void f_runJobs();
        void f_stopJobs();
        void f_resetJobs();
        void f_pauseJobs();

        void f_removeJobs();
        int f_count();

        IJobHandle f_createNew(IJob job);
    }

    public class JobFactory : IJobFactory
    {
        readonly Func<IJobHandle, object, bool> JOB_ACTION = (IJobHandle handle, object para) =>
        {
            JOB_HANDLE_STATE cmd = (JOB_HANDLE_STATE)para;
            switch (cmd)
            {
                case JOB_HANDLE_STATE.PAUSE:
                    handle.f_stopJob();
                    break;
                case JOB_HANDLE_STATE.REMOVE:
                    handle.f_removeJob();
                    break;
                case JOB_HANDLE_STATE.RESET:
                    handle.f_resetJob();
                    break;
                case JOB_HANDLE_STATE.RUN:
                    handle.f_runJob();
                    break;
                case JOB_HANDLE_STATE.STOP:
                    handle.f_stopJob();
                    break;
            }
            return true;
        };

        readonly DictionaryThreadSafe<int, IJobHandle> JobHandles;
        readonly JOB_TYPE JobType;

        public JobFactory(JOB_TYPE jobType)
        {
            this.JobType = jobType;
            JobHandles = new DictionaryThreadSafe<int, IJobHandle>();
        }

        public IJobHandle f_createNew(IJob job)
        {
            // The main thread uses AutoResetEvent to signal the
            // registered wait handle, which executes the callback
            // method: new AutoResetEvent(???)
            //          + true = signaled -> thread continous run
            //          + false = non-signaled -> thread must wait
            //      EventWaitHandle có ba phương thức chính bạn cần quan tâm:
            //      – Close: giải phóng các tài nguyên được sử dụng bởi WaitHandle.
            //      – Reset: chuyển trạng thái của event thành non-signaled.
            //      – Set: chuyển trạng thái của event thành signaled.
            //      – WaitOne([parameters]): Chặn thread hiện tại cho đến khi trạng thái của event được chuyển sang signaled.

            IJobHandle jo = new JobHandle(job, new AutoResetEvent(false));
            int id = job.f_getId();
            JobHandles.Add(id, jo);

            return jo;
        }
         
        public void f_pauseJobs()
        {
            this.JobHandles.ExecuteFunc(JOB_ACTION, JOB_HANDLE_STATE.PAUSE);
        }

        public void f_resetJobs()
        {
            this.JobHandles.ExecuteFunc(JOB_ACTION, JOB_HANDLE_STATE.RESET);
        }

        public void f_runJobs()
        {
            this.JobHandles.ExecuteFunc(JOB_ACTION, JOB_HANDLE_STATE.RUN);
        }

        public void f_stopJobs()
        {
            this.JobHandles.ExecuteFunc(JOB_ACTION, JOB_HANDLE_STATE.STOP);
        }

        public void f_removeJobs()
        {
            this.JobHandles.ExecuteFunc(JOB_ACTION, JOB_HANDLE_STATE.REMOVE);
        }

        public int f_count()
        {
            return JobHandles.Count;
        }

        ~JobFactory()
        {
            f_removeJobs();
        }
    }
}
