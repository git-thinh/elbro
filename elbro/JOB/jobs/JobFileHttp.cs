using System;
using System.Threading;
using System.Linq;
using NHttp;
using System.IO;

namespace elbro
{
    public class JobFileHttp : IJob
    {
        readonly DictionaryThreadSafe<string, string> fileData;
        readonly QueueThreadSafe<Message> msg;
        
        private volatile JOB_STATE _state = JOB_STATE.NONE;
        private volatile JOB_TYPE _type = JOB_TYPE.NONE;

        public JOB_STATE f_getState() { return _state; }
        public JOB_TYPE f_getType() { return _type; }

        public IJobStore StoreJob { get; }
        public void f_stopAndFreeResource() { }
        public void f_sendMessage(Message m) { if (this.StoreJob != null) this.StoreJob.f_job_sendMessage(m); }

        private volatile int Id = 0;
        public int f_getId() { return Id; }
        public void f_setId(int id) { Interlocked.Add(ref Id, id); }
        readonly string _groupName = JOB_NAME.SYS_LINK;
        public string f_getGroupName() { return _groupName; }
        public JobFileHttp(IJobStore _store)
        {
            this.StoreJob = _store;
            msg = new QueueThreadSafe<Message>();
            fileData = new DictionaryThreadSafe<string, string>();
            server = new HttpServer();
            server.RequestReceived += f_server_onRequestReceived;
        }

        private void f_server_onRequestReceived(object sender, HttpRequestEventArgs e)
        {
            string file_name = e.Request.QueryString["file_name"];
            if (!string.IsNullOrEmpty(file_name))
            {
                using (var writer = new StreamWriter(e.Response.OutputStream))
                {
                    writer.Write("Hello world!");
                }
            }
        }

        public bool f_checkKey(object key) { return false; }
        public bool f_setData(string key, object data) { return false; }

        public void f_receiveMessage(Message m)
        {
            msg.Enqueue(m);
        }

        readonly HttpServer server;
        int Port = 0;
        public int f_getPort() { return Port; }
        private void f_Init()
        {
            // Tracer.WriteLine("J{0} executes on thread {1}: INIT ...");
            //Process.Start(String.Format("http://{0}/", server.EndPoint));
            server.Start();
            Port = server.EndPoint.Port;
            Tracer.WriteLine(String.Format("http://{0}/", server.EndPoint));
        }

        private volatile bool _inited = false;
        public void f_stopJob()
        {
            fileData.Clear();
            server.Stop();
            server.Dispose();

            //jobInfo.f_stopJob();
        }

        private JobHandle jobInfo;
        public void f_runLoop(object state, bool timedOut)
        {
            if (!_inited)
            {
                jobInfo = (JobHandle)state;
                _inited = true;
                f_Init();
                return;
            }            
            if (!timedOut)
            {
                // Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP ...", Id, Thread.CurrentThread.GetHashCode().ToString());
                f_stopJob();
                return;
            }

            Tracer.WriteLine("J{0} executes on thread {1}:DO SOMETHING ...", Id, Thread.CurrentThread.GetHashCode().ToString());
            // Do something ...

        }

    }
}
