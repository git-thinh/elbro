using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace elbro
{
    public interface IJobMonitor
    {
        IJobHandle f_createNew(IJob job);
        void f_removeAll();
        void f_runAll();
    }

    public class JobMonitor : IJobMonitor, IJobAction, IMessageEvent
    {
        readonly IJobHandle HandleMessage;
        readonly QueueThreadSafe<Guid> ResponseIds;
        readonly DictionaryThreadSafe<Guid, Message> ResponseMessages;
        readonly DictionaryThreadSafe<JOB_TYPE, IJobFactory> JobFactories;
        readonly DictionaryThreadSafe<JOB_TYPE, IJobHandle> JobSingletons;

        readonly DictionaryThreadSafe<Guid, List<Guid>> RequestMessageGroup;
        readonly DictionaryThreadSafe<Guid, int> RequestMessageGroupTotal;
        readonly DictionaryThreadSafe<Guid, Action> RequestMessageGroupAction;

        public JobMonitor()
        {
            this.ResponseIds = new QueueThreadSafe<Guid>();
            this.ResponseMessages = new DictionaryThreadSafe<Guid, Message>();
            this.RequestMessageGroup = new DictionaryThreadSafe<Guid, List<Guid>>();
            this.RequestMessageGroupTotal = new DictionaryThreadSafe<Guid, int>();
            this.RequestMessageGroupAction = new DictionaryThreadSafe<Guid, Action>();

            this.JobFactories = new DictionaryThreadSafe<JOB_TYPE, IJobFactory>();
            this.JobSingletons = new DictionaryThreadSafe<JOB_TYPE, IJobHandle>();

            this.HandleMessage = f_createNew(new JobMessage(this, this));
            //f_createNew(new JobLink(this));
        }

        public IJobHandle f_createNew(IJob job)
        {
            IJobHandle handle = null;
            JOB_TYPE type = job.f_getType();
            switch (job.f_getType())
            {
                case JOB_TYPE.NONE:
                case JOB_TYPE.REQUEST_URL:
                    // factory
                    IJobFactory fac;
                    if (this.JobFactories.ContainsKey(type))
                    {
                        fac = this.JobFactories[type];
                    }
                    else
                    {
                        fac = new JobFactory(type);
                        this.JobFactories.Add(type, fac);
                    }
                    handle = fac.f_createNew(job);
                    break;
                default:
                    // singleton
                    if (!this.JobSingletons.ContainsKey(type))
                    {
                        handle = new JobHandle(job, new AutoResetEvent(false));
                        this.JobSingletons.Add(type, handle);
                    }
                    break;
            }

            if (handle != null)
            {
                handle.f_runJob();

            }

            return handle;
        }

        public void f_eventJobHandleChangeState(JOB_HANDLE_STATE state, int jobId)
        {
            System.Tracer.WriteLine("MONITOR J{0} = {1}", jobId, state);
        }

        public int f_getTotalJob()
        {
            IJobFactory[] facs = this.JobFactories.ValuesArray;
            return facs.Sum(x => x.f_count()) + this.JobSingletons.Count;
        }

        public void f_removeAll()
        {
            foreach (var kv in this.JobFactories)
                kv.Value.f_actionJobs(JOB_HANDLE_STATE.REMOVE);
            foreach (var kv in this.JobSingletons)
                kv.Value.f_removeJob();
        }

        public void f_runAll()
        {
            foreach (var kv in this.JobFactories)
                kv.Value.f_actionJobs(JOB_HANDLE_STATE.RUN);
            foreach (var kv in this.JobSingletons)
                kv.Value.f_runJob();
        }

        public bool f_setData(JOB_TYPE type, string key, object data)
        {
            return false;
        }

        public void f_eventJobResponseMessage(int jobId, Message m) {
            this.HandleMessage.f_sendMessage(m);

            //Guid id = m.GetMessageId();
            //this.ResponseIds.Enqueue(id);
            //this.ResponseMessages.Add(id, m);
            
            //Guid groupId = m.GetGroupId();
            //if (this.RequestMessageGroup.ContainsKey(groupId)) {
            //    this.RequestMessageGroup.Remove(groupId);
            //    if (this.RequestMessageGroup.Count == 0)
            //    {
            //        System.Tracer.WriteLine("MONITOR DONE GROUP MESSAGES {0} = {1}", groupId, this.RequestMessageGroupTotal[groupId]);
            //        this.RequestMessageGroupAction[groupId]();
            //    }
            //}
        }

        public bool f_requestMessages(JOB_TYPE type, Message[] ms, Action actionCallBackDoneAll = null)
        {
            if (this.JobFactories.ContainsKey(type)) {
                if (actionCallBackDoneAll != null)
                {
                    Guid groupId = Guid.NewGuid();
                    ms = ms.Select(x => x.SetGroupId(groupId)).ToArray();
                    var ids = ms.Select(x => x.GetMessageId()).ToList();
                    this.RequestMessageGroup.Add(groupId, ids);
                    this.RequestMessageGroupTotal.Add(groupId, ids.Count);
                    this.RequestMessageGroupAction.Add(groupId, actionCallBackDoneAll);
                }
                this.JobFactories[type].f_sendRequestLoadBalancer(ms);
            } else if (this.JobSingletons.ContainsKey(type)) {
                this.JobSingletons[type].f_sendMessages(ms);
            }
            return false;
        }

        ~JobMonitor()
        {
            f_removeAll();

            this.ResponseMessages.Clear();
            this.RequestMessageGroup.Clear();

            this.JobFactories.Clear();
            this.JobSingletons.Clear();
        }
    }
}
