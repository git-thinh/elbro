using System;
using System.Threading;

namespace elbro
{
    public interface IMessageEvent {

    }

    public class JobMessage : JobBase
    {
        readonly QueueThreadSafe<Message> Messages;
        readonly IMessageEvent MsgEvent;

        public JobMessage(IJobAction jobAction, IMessageEvent msgEvent) : base(JOB_TYPE.MESSAGE, jobAction)
        {
            this.Messages = new QueueThreadSafe<Message>();
            this.MsgEvent = msgEvent;
        }

        public override void f_sendMessage(Message m)
        {
            this.Messages.Enqueue(m);
        }

        public override void f_receiveMessage(Message m)
        {
        }

        public override void f_sendMessages(Message[] ms)
        {
        }

        public override int f_getPort()
        {
            return 0;
        }
        public override bool f_checkKey(object key)
        {
            return false;
        }
        public override bool f_setData(string key, object data)
        {
            return false;
        }

        public override void f_Init()
        {
            Tracer.WriteLine("J{0} TEST: SIGNAL -> INITED", this.f_getId());
        }

        public override void f_processMessage()
        {
            Message m = null;
            m = this.Messages.Dequeue(null);
            if (m != null)
            {
                
            }
        }

    }
}
