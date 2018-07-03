//using CefSharp;
using Gecko;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace elbro
{
    public class app {

        static app()
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

        static IJobStore jobs;

        public static void f_RUN()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 1000;
            // active SSL 1.1, 1.2, 1.3 for WebClient request HTTPS
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072 | (SecurityProtocolType)0x00000C00 | SecurityProtocolType.Tls;

            Xpcom.Initialize("Bin");
            GeckoPreferences.User["extensions.blocklist.enabled"] = false;
            // Uncomment the follow line to enable error page
            GeckoPreferences.User["browser.xul.error_pages.enabled"] = true;
            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true;
            GeckoPreferences.User["full-screen-api.enabled"] = true;
            // Enable HTML5 Video, Audio: true
            GeckoPreferences.User["media.navigator.permission.disabled"] = false;

            //////var settings = new Settings();
            ////////settings.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
            //////if (!CEF.Initialize(new Settings()))
            //////{
            //////    ////////if (Environment.GetCommandLineArgs().Contains("--type=renderer"))
            //////    ////////{
            //////    ////////    Environment.Exit(0);
            //////    ////////}
            //////    ////////else
            //////    ////////{
            //////    ////////    return;
            //////    ////////}
            //////}

            jobs = new JobStore();
            Application.EnableVisualStyles();


            //Application.Run(new fMedia());
            //Application.Run(new fMain());
            //Application.Run(new fEdit());
            //Application.Run(new fBrowser());
            //Application.Run(new fGeckFX());
            Application.Run(new fApp(jobs));
            f_Exit();
        }

        //public static IFORM get_Main() {
        //    return null;
        //}

        static void f_Exit() { 
            if(Xpcom.IsInitialized)
                Xpcom.Shutdown();
            Application.ExitThread();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Application.Exit();
        }
    }

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
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
            app.f_RUN();

            //test.f_MediaMP3Stream_Demo();
            //test.f_jobTest();
            //test.f_jobWebClient();
            //test.f_jobSpeechEN();
            //test.f_JobGooTranslate();
            //test.f_JobWord();
            //Console.ReadLine();
        }
    }
}
