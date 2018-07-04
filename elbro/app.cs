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
    public class app
    {

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

            f_geckoSetting();

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

        static void f_Exit()
        {
            if (Xpcom.IsInitialized)
                Xpcom.Shutdown();
            Application.ExitThread();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Application.Exit();
        }

        static void f_geckoSetting()
        {
            Xpcom.Initialize("Bin");
            GeckoPreferences.User["extensions.blocklist.enabled"] = false;
            // Uncomment the follow line to enable error page
            GeckoPreferences.User["browser.xul.error_pages.enabled"] = true;
            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true;
            GeckoPreferences.User["full-screen-api.enabled"] = true;
            // Enable HTML5 Video, Audio: true
            GeckoPreferences.User["media.navigator.permission.disabled"] = false;
        }

        static void f_geckoSetting_bak()
        {
            Xpcom.Initialize("Bin");
            GeckoPreferences.User["extensions.blocklist.enabled"] = false;
            // Uncomment the follow line to enable error page
            GeckoPreferences.User["browser.xul.error_pages.enabled"] = true;
            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true;
            GeckoPreferences.User["full-screen-api.enabled"] = true;
            // Enable HTML5 Video, Audio: true
            GeckoPreferences.User["media.navigator.permission.disabled"] = false;

            ////'TEST EDIT USER AGENT
            ////Dim sUserAgent As String = "Mozilla/5.0 (Windows; U; Windows NT 6.1; pl; rv:1.9.1) Gecko/20090624 Firefox/3.5 (.NET CLR 3.5.30729)"
            ////Gecko.GeckoPreferences.User("general.useragent.override") = sUserAgent

            ////'THIS DISABLE THE PDF PLUGIN THAT OPEN THE PDF ON BROWSER
            ////Gecko.GeckoPreferences.User("plugin.disable_full_page_plugin_for_types") = "application/pdf"

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

            // BL-535: 404 error if system proxy settings not configured to bypass proxy for localhost
            // See: https://developer.mozilla.org/en-US/docs/Mozilla/Preferences/Mozilla_networking_preferences
            GeckoPreferences.User["network.proxy.http"] = string.Empty;
            GeckoPreferences.User["network.proxy.http_port"] = 80;
            GeckoPreferences.User["network.proxy.type"] = 1; // 0 = direct (uses system settings on Windows), 1 = manual configuration
            // Try some settings to reduce memory consumption by the mozilla browser engine.
            // Testing on Linux showed eventual substantial savings after several cycles of viewing
            // all the pages and then going to the publish tab and producing PDF files for several
            // books with embedded jpeg files.  (physical memory 1,153,864K, managed heap 37,789K
            // instead of physical memory 1,952,380K, managed heap 37,723K for stepping through the
            // same operations on the same books in the same order.  I don't know why managed heap
            // changed although it didn't change much.)
            // See http://kb.mozillazine.org/About:config_entries, http://www.davidtan.org/tips-reduce-firefox-memory-cache-usage
            // and http://forums.macrumors.com/showthread.php?t=1838393.
            GeckoPreferences.User["memory.free_dirty_pages"] = true;
            GeckoPreferences.User["browser.sessionhistory.max_entries"] = 0;
            GeckoPreferences.User["browser.sessionhistory.max_total_viewers"] = 0;
            GeckoPreferences.User["browser.cache.memory.enable"] = false;

            // Some more settings that can help to reduce memory consumption.
            // (Tested in switching pages in the Edit tool.  These definitely reduce consumption in that test.)
            // See http://www.instantfundas.com/2013/03/how-to-keep-firefox-from-using-too-much.html
            // and http://kb.mozillazine.org/Memory_Leak.
            // maximum amount of memory used to cache decoded images
            GeckoPreferences.User["image.mem.max_decoded_image_kb"] = 40960;        // 40MB (default = 256000 == 250MB)
            // maximum amount of memory used by javascript
            GeckoPreferences.User["javascript.options.mem.max"] = 40960;            // 40MB (default = -1 == automatic)
            // memory usage at which javascript starts garbage collecting
            GeckoPreferences.User["javascript.options.mem.high_water_mark"] = 20;   // 20MB (default = 128 == 128MB)
            // SurfaceCache is an imagelib-global service that allows caching of temporary
            // surfaces. Surfaces normally expire from the cache automatically if they go
            // too long without being accessed.
            GeckoPreferences.User["image.mem.surfacecache.max_size_kb"] = 40960;    // 40MB (default = 102400 == 100MB)
            GeckoPreferences.User["image.mem.surfacecache.min_expiration_ms"] = 500;    // 500ms (default = 60000 == 60sec)

            // maximum amount of memory for the browser cache (probably redundant with browser.cache.memory.enable above, but doesn't hurt)
            GeckoPreferences.User["browser.cache.memory.capacity"] = 0;             // 0 disables feature

            // do these do anything?
            //GeckoPreferences.User["javascript.options.mem.gc_frequency"] = 5; // seconds?
            //GeckoPreferences.User["dom.caches.enabled"] = false;
            //GeckoPreferences.User["browser.sessionstore.max_tabs_undo"] = 0;  // (default = 10)
            //GeckoPreferences.User["network.http.use-cache"] = false;

            // These settings prevent a problem where the gecko instance running the add page dialog
            // would request several images at once, but we were not able to generate the image
            // because we could not make additional requests of the localhost server, since some limit
            // had been reached. I'm not sure all of them are needed, but since in this program we
            // only talk to our own local server, there is no reason to limit any requests to the server,
            // so increasing all the ones that look at all relevant seems like a good idea.
            GeckoPreferences.User["network.http.max-persistent-connections-per-server"] = 200;
            GeckoPreferences.User["network.http.pipelining.maxrequests"] = 200;
            GeckoPreferences.User["network.http.pipelining.max-optimistic-requests"] = 200;

            // This suppresses the normal zoom-whole-window behavior that Gecko normally does when using the mouse while
            // while holding crtl. Code in bloomEditing.js provides a more controlled zoom of just the body.
            GeckoPreferences.User["mousewheel.with_control.action"] = 0;
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
