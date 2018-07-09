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

        IJobHandle f_addJobNew(IJob job);
    }

    public class JobFactory : IJobFactory
    {
        readonly DictionaryThreadSafe<int, AutoResetEvent> JobEvents;
        readonly DictionaryThreadSafe<int, IJobHandle> JobHandles;
        readonly JOB_TYPE JobType;

        public JobFactory(JOB_TYPE jobType) {
            this.JobType = jobType;
            JobEvents = new DictionaryThreadSafe<int, AutoResetEvent>();
            JobHandles = new DictionaryThreadSafe<int, IJobHandle>();
        }

        public IJobHandle f_addJobNew(IJob job)
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
            AutoResetEvent ev = new AutoResetEvent(false);
            IJobHandle jo = new JobHandle(job, ev);
            int id = job.f_getId();

            JobHandles.Add(id, jo);
            JobEvents.Add(id, ev); 

            return jo;
        }

        public void f_pauseJobs()
        {
        }

        public void f_resetJobs()
        {
        }

        public void f_runJobs()
        {
        }

        public void f_stopJobs()
        {
        }

        public void f_removeJobs()
        {
        }

        ~JobFactory()
        {
            f_removeJobs();
        }
    }
}
