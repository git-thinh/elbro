using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace elbro
{
    public interface IJobFactory
    {
        void f_actionJobs(JOB_HANDLE action);
        int f_count();

        IJobHandle f_createNew(IJob job);

        void f_sendRequestLoadBalancer(Message[] messages);
    }

    public class JobFactory : IJobFactory
    {
        static readonly Func<IJobHandle, object, bool> JOB_ACTION = (IJobHandle handle, object para) =>
         {
             JOB_HANDLE cmd = (JOB_HANDLE)para;
             switch (cmd)
             {
                 case JOB_HANDLE.PAUSE:
                     handle.f_stopJob();
                     break;
                 case JOB_HANDLE.REMOVE:
                     handle.f_removeJob();
                     break;
                 case JOB_HANDLE.RESET:
                     handle.f_resetJob();
                     break;
                 case JOB_HANDLE.RUN:
                     handle.f_runJob();
                     break;
                 case JOB_HANDLE.STOP:
                     handle.f_stopJob();
                     break;
             }
             return true;
         };

        static readonly Func<IJobHandle[], Message[], bool> SEND_MESSAGE_LOAD_BALANCER_TO_JOB = (IJobHandle[] handles, Message[] ms) =>
        {
            int count = ms.Length, i = 0, id = 0;
            while (count > 0)
            {
                i = 0;
                for (i = 0; i < handles.Length; i++)
                {
                    ms[id].f_setJobExecuteId(handles[i].f_getJob().f_getId());
                    handles[i].f_sendMessage(ms[id]);
                    System.Tracer.WriteLine(String.Format("J{0}: message-{1}: {2}", handles[i].f_getJob().f_getId(), id, ms[id].GetMessageId()));

                    id++;
                    if (id == ms.Length)
                    {
                        count = 0;
                        break;
                    }
                }
                count = count - i;
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

        public int f_count()
        {
            return JobHandles.Count;
        }

        public void f_actionJobs(JOB_HANDLE action)
        {
            this.JobHandles.ExecuteFunc(JOB_ACTION, action);
        }

        public void f_sendRequestLoadBalancer(Message[] messages)
        {
            this.JobHandles.ExecuteFuncLoadBalancer<Message>(SEND_MESSAGE_LOAD_BALANCER_TO_JOB, messages);
        }

        ~JobFactory()
        {
            this.JobHandles.ExecuteFunc(JOB_ACTION, JOB_HANDLE.REMOVE);
        }
    }
}
