using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace elbro
{
    public interface IJobContext {
        int f_getTotalJob();
        IMessageContext MessageContext { get; }

        void f_jobSingletonStateChanged(int jobId, JOB_HANDLE state);
    }
    
    public interface IJobFactory
    {
        void f_actionJobs(JOB_HANDLE action);
        int f_count();

        IJobHandle f_createNew(IJob job);

        void f_sendRequestLoadBalancer(Message[] messages);

        void f_jobFactoryStateChanged(int jobId, JOB_HANDLE state);
    }

    public interface IJobHandle
    {
        IJob Job { get; }
        IJobFactory Factory { get; }
        JOB_HANDLE State { get; }
        
        void f_actionJob(JOB_HANDLE action);
        void f_actionJobCallback();

        void f_receiveMessage(Message m);
        void f_receiveMessages(Message[] ms);
    }
    
    public interface IJob
    {
        int f_getId();

        IJobHandle Handle { get; }
        IJobContext JobContext { get; }
        JOB_STATE State { get; }
        JOB_TYPE Type { get; }

        void f_receiveMessage(Message m);
        void f_receiveMessages(Message[] ms);
        
        void f_runLoop(IJobHandle handle);
    }
    
    public interface IMessageContext
    {
        void f_responseMessage(Message m);
        Message f_responseMessageGet(Guid id);

        void f_sendRequestMessages(JOB_TYPE type, Message[] messages);

        //void f_eventRequestGroupMessageComplete(Guid groupId);
        //void f_eventRequestMessageTimeOut(Guid[] IdsExpired);
    }

}
