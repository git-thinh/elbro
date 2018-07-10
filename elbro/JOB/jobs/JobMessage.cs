using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace elbro
{
    public interface IMessageContext {
        void f_eventRequestGroupMessageComplete(Guid groupId);
    }

    public class JobMessage : JobBase
    {
        public const string REQUEST_MSG_GROUP = "REQUEST_MSG_GROUP";

        readonly QueueThreadSafe<Message> Messages;
        readonly IMessageContext MsgEvent;

        readonly QueueThreadSafe<Guid> ResponseIds;
        readonly DictionaryThreadSafe<Guid, Message> ResponseMessages;
        readonly DictionaryThreadSafe<Guid, List<Guid>> RequestMessageGroup;
        readonly DictionaryThreadSafe<Guid, int> RequestMessageGroupTotal;
        readonly DictionaryThreadSafe<Guid, Func<IJobAction, Guid, Guid[], IJobHandle, bool>> RequestMessageGroupAction;

        public JobMessage(IJobAction jobAction, IMessageContext msgEvent) : base(JOB_TYPE.MESSAGE, jobAction)
        {
            this.Messages = new QueueThreadSafe<Message>();
            this.MsgEvent = msgEvent;

            this.ResponseIds = new QueueThreadSafe<Guid>();
            this.ResponseMessages = new DictionaryThreadSafe<Guid, Message>();
            this.RequestMessageGroup = new DictionaryThreadSafe<Guid, List<Guid>>();
            this.RequestMessageGroupTotal = new DictionaryThreadSafe<Guid, int>();
            this.RequestMessageGroupAction = new DictionaryThreadSafe<Guid, Func<IJobAction, Guid, Guid[], IJobHandle, bool>>();
        }

        public override void f_sendMessage(Message m)
        {
            this.Messages.Enqueue(m);
        }

        public override void f_receiveMessage(Message m)
        {
        }

        public override bool f_checkKey(object key)
        {
            return false;
        }
        public override bool f_setData(string key, object data)
        {
            switch (key) {
                case REQUEST_MSG_GROUP:
                    var para = (Tuple<Func<IJobAction, Guid, Guid[], IJobHandle, bool>, Message[]>)data;
                    Message[] ms = para.Item2;
                    Guid groupId = Guid.NewGuid();

                    ms = ms.Select(x => x.SetGroupId(groupId)).ToArray();
                    var ids = ms.Select(x => x.GetMessageId()).ToList();

                    this.RequestMessageGroup.Add(groupId, ids);
                    this.RequestMessageGroupTotal.Add(groupId, ids.Count);
                    this.RequestMessageGroupAction.Add(groupId, para.Item1);

                    break;
            }
            return false;
        }

        public override void f_Init()
        {
            Tracer.WriteLine("J{0} JOB_MESSAGE: SIGNAL -> INITED", this.f_getId());
        }

        public override void f_processMessage()
        {
            Message m = null;
            m = this.Messages.Dequeue(null);
            if (m != null)
            {
                Guid id = m.GetMessageId();
                this.ResponseIds.Enqueue(id);
                this.ResponseMessages.Add(id, m);

                Guid groupId = m.GetGroupId();
                if (this.RequestMessageGroup.ContainsKey(groupId))
                {
                    this.RequestMessageGroup.Remove(groupId);
                    if (this.RequestMessageGroup.Count == 0)
                    {
                        System.Tracer.WriteLine("JOB_MESSAGE:  DONE GROUP {0} = {1}", groupId, this.RequestMessageGroupTotal[groupId]);
                        this.RequestMessageGroupAction[groupId](this.JobAction, groupId, null, null);
                    }
                }
            }
        }

        ~JobMessage()
        {
            this.ResponseMessages.Clear();
            this.RequestMessageGroup.Clear();
        }
    }
}
