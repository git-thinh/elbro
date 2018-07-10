using System;
using System.Threading;

namespace elbro
{
    public class JobTest : JobBase
    {
        public JobTest(IJobAction jobAction) : base(JOB_TYPE.NONE, jobAction)
        {
        } 

        public override void f_sendMessage(Message m)
        {
        }
        public override void f_receiveMessage(Message m)
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

        public override void f_Init() {
                Tracer.WriteLine("J{0} TEST: SIGNAL -> INITED", this.f_getId());
        }

        public override void f_processMessage()
        {
            Tracer.WriteLine("J{0} TEST: Do something ...", this.f_getId());
        }
        
    }
}
