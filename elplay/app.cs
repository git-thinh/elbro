using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace elplay
{
    [PermissionSet(SecurityAction.LinkDemand, Name = "Everything"),
    PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    public class app
    {
        #region [ VARIABLE ]

        public const int m_item_width = 120;
        public const int m_item_height = 90;

        public const int m_box_width = 320;
        public const int m_box_height = 180;

        public const int m_app_width = m_box_width * 2 + 29;
        public const int m_app_height = 569;

        public const int m_player_width = 640;
        public const int m_player_height = 360;
        
        static fPlayer media;

        #endregion
         
        static void f_player_init()
        {
            media = new fPlayer();
            media.FormBorderStyle = FormBorderStyle.None;
            media.ShowInTaskbar = false;
            media.StartPosition = FormStartPosition.Manual;
            media.Shown += (se, ev) =>
            {
                media.Top = (Screen.PrimaryScreen.WorkingArea.Height - app.m_player_height) / 2;
                media.Left = (Screen.PrimaryScreen.WorkingArea.Width - app.m_player_width) / 2;

                media.f_active();
            };
            media.Width = app.m_app_width;
            //media.Width = 1;
            //media.Location = new System.Drawing.Point(-2000, -2000);
            //media.Show();
            //media.Hide();

            Application.EnableVisualStyles();
            Application.Run(media);
        }

        public static void f_player_Open(string url, string title)
        {
            media.Invoke((Action)(() =>
            {
                media.Show();
                media.ShowInTaskbar = true;
                media.open(url, title);
            }));
        }

        public static void f_player_Hide()
        {
            media.ShowInTaskbar = false;
            media.Hide();
        }

        public static void f_player_Close()
        {
            f_EXIT();
        }
        
        static app()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (se, ev) =>
            {
                Assembly asm = null;
                string comName = ev.Name.Split(',')[0];

                //string resourceName = @"dll\" + comName + ".dll";
                //var assembly = Assembly.GetExecutingAssembly();
                //resourceName = typeof(app).Namespace + "." + resourceName.Replace(" ", "_").Replace("\\", ".").Replace("/", ".");
                //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (Stream stream = File.OpenRead("bin/" + comName + ".dll"))
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
            f_player_init();
        }

        public static void f_EXIT()
        {
            media.f_form_freeResource();
            media.Close();

            Application.ExitThread();
            // wait for complete threads, free resource
            //Thread.Sleep(30);
            Application.Exit();
        }
    }

    class Program
    { 
        [STAThread]
        static void Main(string[] args)
        {
            app.f_INIT();
            app.f_RUN();
            app.f_EXIT();
        }
    }
}
