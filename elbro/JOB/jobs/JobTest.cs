﻿using System;
using System.Threading;

namespace elbro
{
    public class JobTest : JobBase
    {
        readonly QueueThreadSafe<Message> Messages;

        public JobTest(IJobAction jobAction) : base(JOB_TYPE.NONE, jobAction)
        {
            this.Messages = new QueueThreadSafe<Message>();
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
                if (m.f_getTimeOut() > 0)
                    Thread.Sleep(m.f_getTimeOut() + 3000);

                Tracer.WriteLine("J{0} TEST: Do something: {1} ", this.f_getId(), m.GetMessageId());
                
                m.Output = new MessageResult()
                {
                    Ok = true,
                    MessageText = string.Format("J{0} TEST: Done {1} ", this.f_getId(), m.GetMessageId())
                };
                this.JobAction.f_eventJobResponseMessage(this.f_getId(), m);
            }
            //else Tracer.WriteLine("J{0} TEST: waiting message to execute that ...", this.f_getId());
        }

    }
}
