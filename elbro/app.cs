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
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | (SecurityProtocolType)3072 | (SecurityProtocolType)0x00000C00 | SecurityProtocolType.Tls;
            }
            catch {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls;
            }

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

            //Also, if you want to disable cache, you can do that:
            //GeckoPreferences.User["browser.cache.disk.enable"] = false;
            GeckoPreferences.User["places.history.enabled"] = false;

            // disable caching of http documents
            GeckoPreferences.User["network.http.use-cache"] = false;
            // disalbe memory caching
            GeckoPreferences.User["browser.cache.memory.enable"] = false;
            // maximum amount of memory for the browser cache (probably redundant with browser.cache.memory.enable above, but doesn't hurt)
            GeckoPreferences.User["browser.cache.memory.capacity"] = 0;             // 0 disables feature

            string gecko_cache_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cache");
            if (!Directory.Exists(gecko_cache_path)) Directory.CreateDirectory(gecko_cache_path);

            // At first I suspected this was due to the profile not having a place to be created, 
            // since the screensaver runs as LOCAL SERVICE. So I added the following to my code:
            // Initialize Gecko, disable caches, point profile at common folder.
            Xpcom.ProfileDirectory = gecko_cache_path;
            GeckoPreferences.Default["browser.cache.disk.parent_directory"] = gecko_cache_path;
            GeckoPreferences.Default["browser.cache.disk.enable"] = false;
            GeckoPreferences.Default["Accessibility.disablecache"] = true;
            GeckoPreferences.Default["browser.cache.offline.enable"] = false;

            // So the profile is being created in the appropriate spot on disk, now.
            // However, I'm still getting the privilege error. 
            // I've granted Everyone full access to the gecko_cache_path and xulrunner_path. 
            // I'm having a hard time determining what additional permissions dependencies this thing needs to work. 
            // It occurred to me that it might have to do with Direct3D being unavailable, so I also added:

            GeckoPreferences.Default["layers.accelerate-all"] = false;
            GeckoPreferences.Default["layers.accelerate-none"] = true;
            GeckoPreferences.Default["layers.acceleration.disabled"] = true;
            GeckoPreferences.Default["gfx.direct2d.disabled"] = true;
            GeckoPreferences.Default["gfx.direct3d.disabled"] = true;
            GeckoPreferences.Default["gfx.content.azure.enable"] = false;
            GeckoPreferences.Default["gfx.direct2d.force-enabled"] = false;
            GeckoPreferences.Default["webgl.disabled"] = true;
        }

        static void f_geckoSetting_bak2()
        {
            //Xpcom.Initialize();

            //// Set some preferences
            //nsIPrefBranch pref = Xpcom.GetService<nsIPrefBranch>("@mozilla.org/preferences-service;1");

            //// Show same page as firefox does for unsecure SSL/TLS connections ...
            //pref.SetIntPref("browser.ssl_override_behavior", 1);
            //pref.SetIntPref("security.OCSP.enabled", 0);
            //pref.SetBoolPref("security.OCSP.require", false);
            //pref.SetBoolPref("extensions.hotfix.cert.checkAttributes", true);
            //pref.SetBoolPref("security.remember_cert_checkbox_default_setting", true);
            //pref.SetBoolPref("services.sync.prefs.sync.security.default_personal_cert", true);
            //pref.SetBoolPref("browser.xul.error_pages.enabled", true);
            //pref.SetBoolPref("browser.xul.error_pages.expert_bad_cert", false);

            //// disable caching of http documents
            //pref.SetBoolPref("network.http.use-cache", false);

            //// disalbe memory caching
            //pref.SetBoolPref("browser.cache.memory.enable", false);

            //// Desktop Notification
            //pref.SetBoolPref("notification.feature.enabled", true);

            //// WebSMS
            //pref.SetBoolPref("dom.sms.enabled", true);
            //pref.SetCharPref("dom.sms.whitelist", "");

            //// WebContacts
            //pref.SetBoolPref("dom.mozContacts.enabled", true);
            //pref.SetCharPref("dom.mozContacts.whitelist", "");

            //pref.SetBoolPref("social.enabled", false);

            //// WebAlarms
            //pref.SetBoolPref("dom.mozAlarms.enabled", true);

            //// WebSettings
            //pref.SetBoolPref("dom.mozSettings.enabled", true);

            //pref.SetBoolPref("network.jar.open-unsafe-types", true);
            //pref.SetBoolPref("security.warn_entering_secure", false);
            //pref.SetBoolPref("security.warn_entering_weak", false);
            //pref.SetBoolPref("security.warn_leaving_secure", false);
            //pref.SetBoolPref("security.warn_viewing_mixed", false);
            //pref.SetBoolPref("security.warn_submit_insecure", false);
            //pref.SetIntPref("security.ssl.warn_missing_rfc5746", 1);
            //pref.SetBoolPref("security.ssl.enable_false_start", false);
            //pref.SetBoolPref("security.enable_ssl3", true);
            //pref.SetBoolPref("security.enable_tls", true);
            //pref.SetBoolPref("security.enable_tls_session_tickets", true);
            //pref.SetIntPref("privacy.popups.disable_from_plugins", 2);

            //// don't store passwords
            //pref.SetIntPref("security.ask_for_password", 1);
            //pref.SetIntPref("security.password_lifetime", 0);
            //pref.SetBoolPref("signon.prefillForms", false);
            //pref.SetBoolPref("signon.rememberSignons", false);
            //pref.SetBoolPref("browser.fixup.hide_user_pass", false);
            //pref.SetBoolPref("privacy.item.passwords", true);

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
