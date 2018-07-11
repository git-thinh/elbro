using System;
using System.Threading;

namespace elbro
{
    public class JobTest : JobBase
    {
        public JobTest(IJobContext jobContext) : base(jobContext, JOB_TYPE.NONE)
        {
        }
        
        public override void f_init()
        {
            Tracer.WriteLine("J{0} TEST: SIGNAL -> INITED", this.f_getId());
        }
        public override void f_processMessageCallbackResult(Guid messageId) {
            Thread.Sleep(1000);
        }
        public override Guid f_processMessage(Message m)
        {
            //Message m = null;
            //m = this.Messages.Dequeue(null);
            //if (m != null)
            //{
            //    if (m.f_getTimeOut() > 0)
            //        Thread.Sleep(m.f_getTimeOut() + 3000);

            //    Tracer.WriteLine("J{0} TEST: Do something: {1} ", this.f_getId(), m.GetMessageId());

            //    m.Output = new MessageResult()
            //    {
            //        Ok = true,
            //        MessageText = string.Format("J{0} TEST: Done {1} ", this.f_getId(), m.GetMessageId())
            //    };
            //    //this.JobAction.f_eventJobResponseMessage(this.f_getId(), m);
            //    this.JobContext.MessageContext.f_responseMessage(m);
            //}
            //else Tracer.WriteLine("J{0} TEST: waiting message to execute that ...", this.f_getId());
            return Guid.Empty;
        }

    }
}
