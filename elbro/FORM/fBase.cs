using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace elbro
{
    public class fBase : Form, IFORM //, IMessageFilter
    {
        public IJobStore JobStore { get; }
        public event EventReceiveMessage OnReceiveMessage;

        readonly QueueThreadSafe<Message> StoreMessages;
        readonly System.Threading.Timer timer_api = null;

        public fBase(IJobStore store)
        {
            StoreMessages = new QueueThreadSafe<Message>();
            JobStore = store;
            store.f_form_Add(this);
            this.FormClosing += (se, ev) => { store.f_form_Remove(this); };

            timer_api = new System.Threading.Timer(new System.Threading.TimerCallback((obj) =>
            {
                IFORM form = (IFORM)obj;
                if (StoreMessages.Count > 0)
                {
                    Message m = StoreMessages.Dequeue(null);
                    if (m != null)
                        OnReceiveMessage?.Invoke(form, m);
                }
            }), this, 100, 100);
        }

        public void f_receiveMessage(Guid id)
        {
            Message m = this.JobStore.f_msg_getMessageData(id);
            if (m != null) StoreMessages.Enqueue(m);
        }

        public void f_sendRequestMessage(Message m)
        {
            this.JobStore.f_job_sendMessage(m);
        }

        public bool f_sendRequestToJob(string job_name, MESSAGE_ACTION action, object input)
        {
            int[] job_IDs = this.JobStore.f_job_getIdsByName(job_name);
            if (job_IDs.Length > 0)
            {
                Message m = new Message(this.f_getFormID(), job_IDs, action, input);
                this.JobStore.f_job_sendMessage(m);
                return true;
            }
            return false;
        }

        public int f_getFormID()
        {
            return this.Handle.ToInt32();
        }

        #region [ LOG ]

        public void f_log(string message)
        {
            System.Tracer.WriteLine(message);
        }

        public void f_log(string format, object arg0)
        {
            System.Tracer.WriteLine(format, arg0);
        }

        public void f_log(string format, object arg0, object arg1)
        {
            System.Tracer.WriteLine(format, arg0, arg1);
        }

        public void f_log(string format, object arg0, object arg1, object arg2)
        {
            System.Tracer.WriteLine(format, arg0, arg1, arg2);
        }

        public void f_log(string format, object arg0, object arg1, object arg2, object arg3)
        {
            System.Tracer.WriteLine(format, arg0, arg1, arg2, arg3);
        }

        #endregion
    }
}
