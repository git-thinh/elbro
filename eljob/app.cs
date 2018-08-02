﻿using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace eljob
{
    public class app
    {
        static app()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];

                string resourceName = @"dll\" + comName + ".dll";
                var assembly = Assembly.GetExecutingAssembly();
                resourceName = typeof(app).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                //using (Stream stream = File.OpenRead("bin/" + comName + ".dll"))
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

        //static JobMonitor jom; 

        public static void f_INIT()
        {
            ThreadPool.SetMaxThreads(25, 25);

            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
            try
            {
                // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                    | (SecurityProtocolType)3072
                    | (SecurityProtocolType)0x00000C00
                    | SecurityProtocolType.Tls;
            }
            catch
            {
                // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
                    | SecurityProtocolType.Tls;
            }
        }

        public static void f_RUN()
        {
            //jom = new JobMonitor();
            //Application.EnableVisualStyles();


            //Application.Run(new fMedia());
            //Application.Run(new fMain());
            //Application.Run(new fEdit());
            //Application.Run(new fBrowser());
            //Application.Run(new fGeckFX());
            //main = new fApp(jobs);
            //main = new fNone(jom);
            //Application.Run(main);
            //f_Exit();

            f_publish_Init();
        }

        #region [ PUBLISH ]

        private static host.Http.HttpListener server;
        private static X509Certificate2 _cert;


        static void f_publish_Init()
        {
            //using (var context = new Pluralsight.Crypto.CryptContext())
            //{
            //    context.Open();

            //    var properties = new Pluralsight.Crypto.SelfSignedCertProperties()
            //    {
            //        IsPrivateKeyExportable = true,
            //        KeyBitLength = 2048,
            //        Name = new X500DistinguishedName("cn=localhost"),
            //        ValidFrom = DateTime.Today.AddDays(-1),
            //        ValidTo = DateTime.Today.AddYears(1)
            //    };

            //    _cert = context.CreateSelfSignedCertificate(properties);
            //}
            //_cert = new X509Certificate2("demo.pfx");

            byte[] buffer = null;
            string resourceName = @"dll\demo.pfx";
            var assembly = Assembly.GetExecutingAssembly();
            resourceName = typeof(app).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName)) 
            {
                if (stream != null) 
                {
                    buffer = new byte[stream.Length];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                            ms.Write(buffer, 0, read);
                        buffer = ms.ToArray();
                    } 
                }
            }
            _cert = new X509Certificate2(buffer);

            server = host.Http.HttpListener.Create(IPAddress.Any, 443, _cert);
            server.RequestReceived += OnSecureRequest;
            server.Start(5);
        }


        static void OnSecureRequest(object source, host.Http.RequestEventArgs args)
        {
            host.Http.IHttpClientContext context = (host.Http.IHttpClientContext)source;
            host.Http.IHttpRequest request = args.Request;

            // Here we create a response object, instead of using the client directly.
            // we can use methods like Redirect etc with it,
            // and we dont need to keep track of any headers etc.
            host.Http.IHttpResponse response = request.CreateResponse(context);

            byte[] body = Encoding.UTF8.GetBytes("Hello secure you! " + DateTime.Now.ToString());
            response.Body.Write(body, 0, body.Length);
            response.Send();
        }


        #endregion

        //public static IFORM get_Main() {
        //    return null;
        //}

        public static void f_EXIT()
        {
            server.Stop();

            //jom.f_removeAll();

            //if (Xpcom.IsInitialized)
            //    Xpcom.Shutdown();
            //Application.ExitThread();

            //GC.Collect();
            //GC.WaitForPendingFinalizers();

            //Application.Exit();
        }

    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            app.f_INIT();
            app.f_RUN();

            //test_job.f_rpc_Handle();
            //test_job.f_websocket_Handle();
            //test_job.f_JobTestRequestUrl();
            //test_job.f_handle_HTTP_FILE();
            //test_job.f_jobTest_Handle();
            //test_job.f_jobTest_Factory();

            Console.ReadLine();
            app.f_EXIT();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
