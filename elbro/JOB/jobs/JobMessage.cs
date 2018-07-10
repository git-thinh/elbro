using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace elbro
{
    public interface IJobMessageContext
    {
        void f_eventRequestGroupMessageComplete(Guid groupId);
    }

    public interface IJobMessage
    {
        void f_eventRequestGroupMessageComplete(Guid groupId);
    }

    public class JobMessage : JobBase
    {
        public const string REQUEST_MSG_GROUP = "REQUEST_MSG_GROUP";

        readonly QueueThreadSafe<Message> Messages;
        readonly IJobMessageContext MsgContext;

        readonly QueueThreadSafe<Guid> ResponseIds;
        readonly DictionaryThreadSafe<Guid, Message> ResponseMessages;
        readonly DictionaryThreadSafe<Guid, List<Guid>> RequestMessageGroup;
        readonly DictionaryThreadSafe<Guid, int> RequestMessageGroupTotal;
        readonly DictionaryThreadSafe<Guid, Func<IJobMessageContext, IJobHandle, Guid, bool>> RequestMessageGroupAction
            = new DictionaryThreadSafe<Guid, Func<IJobMessageContext, IJobHandle, Guid, bool>>();

        public JobMessage(IJobAction jobAction, IJobMessageContext msgContext) : base(JOB_TYPE.MESSAGE, jobAction)
        {
            this.Messages = new QueueThreadSafe<Message>();
            this.MsgContext = msgContext;

            this.ResponseIds = new QueueThreadSafe<Guid>();
            this.ResponseMessages = new DictionaryThreadSafe<Guid, Message>();
            this.RequestMessageGroup = new DictionaryThreadSafe<Guid, List<Guid>>();
            this.RequestMessageGroupTotal = new DictionaryThreadSafe<Guid, int>();
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
                    var para = (Tuple<Func<IJobMessageContext, IJobHandle, Guid, bool>, Message[]>)data;
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
                        this.RequestMessageGroupAction[groupId](this.MsgContext, this.f_getHandle(), groupId);
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
