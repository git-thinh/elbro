﻿using NAudio;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Speech.Synthesis;
using System.Threading;

namespace corel
{
    public class JobGooTranslate : IJob
    { 
        readonly QueueThreadSafe<string> queue;
        readonly DictionaryThreadSafe<string, string> storeUrl;
        readonly DictionaryThreadSafe<string, string> storePath;

        static JobGooTranslate()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];
                string resourceName = @"DLL\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(app).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        //Debug.WriteLine(resourceName);
                    }
                    else
                    {
                        byte[] buffer = new byte[stream.Length];
                        using (MemoryStream ms = new MemoryStream())
                        {
                            int read;
                            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                                ms.Write(buffer, 0, read);
                            buffer = ms.ToArray();
                        }
                        asm = Assembly.Load(buffer);
                    }
                }
                return asm;
            };
        }


        private volatile JOB_STATE _state = JOB_STATE.NONE;
        private volatile JOB_TYPE _type = JOB_TYPE.NONE;

        public JOB_STATE f_getState() { return _state; }
        public JOB_TYPE f_getType() { return _type; }

        public IJobStore StoreJob { get; }
        private volatile int Id = 0;
        public int f_getId() { return Id; }
        public int f_getPort() { return 0; }
        public bool f_checkKey(object key) { return false; }
        public bool f_setData(string key, object data) { return false; }
        public void f_setId(int id) { Interlocked.Add(ref Id, id); }
        readonly string _groupName = string.Empty;
        public string f_getGroupName() { return _groupName; }
        public JobGooTranslate(IJobStore _store)
        {
            this.StoreJob = _store;
            this.queue = new QueueThreadSafe<string>();
            this.storeUrl = new DictionaryThreadSafe<string, string>();
            this.storePath = new DictionaryThreadSafe<string, string>(); 
        }
        public void f_receiveMessage(Message m) { }
        public void f_sendMessage(Message m) { if (this.StoreJob != null) this.StoreJob.f_job_sendMessage(m); }

        private volatile bool _inited = false;
        public void f_stopJob()
        { 
            jobInfo.f_stopJob();
        }

        private JobHandle jobInfo;
        public void f_runLoop(object state, bool timedOut)
        {
            if (!_inited)
            {
                jobInfo = (JobHandle)state;
                _inited = true; 
                return;
            }
            if (!timedOut)
            {
            System.Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP", Id, Thread.CurrentThread.GetHashCode().ToString());
                // Tracer.WriteLine("J{0} executes on thread {1}: SIGNAL -> STOP ...", Id, Thread.CurrentThread.GetHashCode().ToString());
                f_stopJob();
                return;
            } 


            if (this.queue.Count > 0)
            {
                string s = this.queue.Dequeue(string.Empty);
                if (s.Length > 0)
                {
                    test_run_v1(s);
                    //test_run_v2(s);

                    System.Tracer.WriteLine("J{0} executes on thread {1}: Speech = {2}", Id, Thread.CurrentThread.GetHashCode().ToString(), s);
                }
            }
        }

        #region [ TEST ]

        void test_run_v1(string text)
        {
            //IsBusy(true);
            GooTranslateService_v1.TranslateAsync(
                text, "en", "vi", string.Empty,
                (success, result, type) =>
                {
                    //SetResult(result, type);
                    //IsBusy(false);
                    Console.WriteLine("\r\n -> " + text + " (" + type + "): " + result);
                    Tracer.WriteLine(text + "(" + type + "): " + result);
                });
        }

        void test_run_v2(string text)
        {
            //IsBusy(true);
            GooTranslateService_v2.TranslateAsync(
                text, "en", "vi", string.Empty,
                (success, result, type) =>
                {
                    //SetResult(result, type);
                    //IsBusy(false);
                    Console.WriteLine("\r\n -> " + text + " (" + type + "): " + result);
                    Tracer.WriteLine(text + "(" + type + "): " + result);
                });
        }

        #endregion 
    }
}
