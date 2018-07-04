using FarsiLibrary.Win;
using Gecko;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;
using Gecko.Events;

namespace elbro
{
    public class fApp : fBase
    {
        readonly Font font_Title = new Font("Arial", 11f, FontStyle.Regular);
        readonly Font font_TextView = new Font("Courier New", 11f, FontStyle.Regular);
        readonly Font font_LogView = new Font("Courier New", 9f, FontStyle.Regular);

        void f_event_OnReceiveMessage(IFORM form, Message m)
        {
            switch (m.JobName)
            {
                case JOB_NAME.SYS_LINK:
                    switch (m.getAction())
                    {
                        case MESSAGE_ACTION.ITEM_SEARCH:
                            break;
                        case MESSAGE_ACTION.URL_REQUEST_CACHE:
                            break;
                    }
                    break;
            }
        }

        #region [ === FORM === ]

        public fApp(IJobStore store) : base(store)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = "English";
            this.OnReceiveMessage += f_event_OnReceiveMessage;
            this.Shown += f_form_Shown;
            this.FormClosing += f_form_Closing;

            f_brow_Init();
            f_tab_Init();
        }

        private void f_form_Closing(object sender, FormClosingEventArgs e)
        {
            f_brow_Close();
        }

        private void f_form_Shown(object sender, EventArgs e)
        {
            //f_brow_Go(brow_URL);
        }

        #endregion

        #region [ === BROWSER === ]

        const string DOMAIN_GOOGLE = "www.google.com.vn";
        const string DOMAIN_BING = "www.bing.com";
        const string DOM_CONTENT_LOADED = "DOM_CONTENT_LOADED";
        const int TOOLBAR_HEIGHT = 28;
        const int SHORTCUTBAR_HEIGHT = 17;

        //string brow_URL = "https://www.google.com.vn";
        //string brow_URL = "https://dictionary.cambridge.org/grammar/british-grammar/do-or-make";
        string brow_URL = "https://dictionary.cambridge.org";
        //string brow_URL = "https://www.bing.com";
        //string brow_URL = "https://www.bing.com/search?go=Submit&qs=ds&form=QBLH&q=hello";
        //string brow_URL = "https://developers.google.com/web/tools/chrome-devtools/network-performance/";
        //string brow_URL = "https://www.google.com/maps";
        //string brow_URL = "http://web20office.com/crm/demo/system/login.php?r=/crm/demo";
        //string brow_URL = "file:///G:/_EL/Document/data_el2/book/84-cau-truc-va-cau-vi-du-thong-dung-trong-tieng-anh-giao-tiep.pdf";
        //string brow_URL = "https://www.youtube.com/";
        //string brow_URL = "https://drive.google.com/open?id=1TG-FDU0cZ48vaJCMcAO33iNOuNqgL9BH";
        //string brow_URL = "https://drive.google.com/open?id=1B_DuOqTAQOcZjuls6bw9Tnx_0nd8qpr8";
        //string brow_URL = "https://drive.google.com/file/d/1B_DuOqTAQOcZjuls6bw9Tnx_0nd8qpr8/view";
        //string brow_URL = "https://drive.google.com/file/d/1TG-FDU0cZ48vaJCMcAO33iNOuNqgL9BH/view";

        string brow_Domain;
        bool brow_ImportPlugin = false,
            brow_EnabelJS = true,
            brow_EnableCSS = false,
            brow_EnableImg = false,
            brow_AutRequest = false;

        TextBoxWaterMark brow_UrlTextBox;
        GeckoWebBrowser browser;
        ControlTransparent brow_Transparent;
        Panel brow_ShortCutBar;

        void f_brow_Init()
        {
            brow_Domain = brow_URL.Split('/')[2];
            browser = new GeckoWebBrowser();
            browser.Dock = DockStyle.Fill;
            //browser.NavigationError += (s, e) =>
            //{
            //    Debug.WriteLine("StartDebugServer error: 0x" + e.ErrorCode.ToString("X"));
            //    browser.Dispose();
            //};
            //browser.DocumentCompleted += (s, e) =>
            //{
            //    Debug.WriteLine("StartDebugServer completed");
            //    //browser.Dispose();
            //};
            browser.UseHttpActivityObserver = false;
            //browser.ObserveHttpModifyRequest += (sender, e) => e.Channel.SetRequestHeader(name, value, merge: true);
            //browser.ObserveHttpModifyRequest += f_brow_ObserveHttpModifyRequest;
            browser.DOMContentLoaded += (se, ev) => { GeckoWebBrowser w = (GeckoWebBrowser)se; if (w != null) f_brow_onDOMContentLoaded(w.DocumentTitle, w.Url); };
            browser.Navigating += (se, ev) => { f_brow_onBeforeNavigating(ev.Uri); };
            browser.ConsoleMessage += (se, ev) => { f_brow_onConsoleMessage(ev.Message); };

            brow_Transparent = new ControlTransparent()
            {
                Location = new Point(0, 0),
                Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };

            Panel toolbar = new Panel() { Dock = DockStyle.Top, Height = TOOLBAR_HEIGHT, BackColor = SystemColors.Control, Padding = new Padding(3, 3, 0, 3) };
            brow_ShortCutBar = new Panel() { Dock = DockStyle.Top, Height = SHORTCUTBAR_HEIGHT, BackColor = SystemColors.Control, Padding = new Padding(0) };

            brow_UrlTextBox = new TextBoxWaterMark()
            {
                WaterMark = "HTTP://...",
                Dock = DockStyle.Fill,
                Height = 20,
                Font = font_Title,
                BorderStyle = BorderStyle.None,
                BackColor = SystemColors.Control,
                ForeColor = Color.DarkGray,
            };
            brow_UrlTextBox.KeyDown += (se, ev) =>
            {
                if (ev.KeyCode == Keys.Enter)
                {
                    f_brow_Go(brow_UrlTextBox.Text.Trim());
                }
            };
            brow_UrlTextBox.MouseDoubleClick += (se, ev) =>
            {
                brow_UrlTextBox.Text = string.Empty;
            };

            var btn_ToggleTab = new Button() { Text = tab_IconToggle, Width = 19, Height = 20, Dock = DockStyle.Right };
            btn_ToggleTab.MouseClick += (se, ev) => { f_tab_Toggle(); };
            toolbar.Controls.AddRange(new Control[] { brow_UrlTextBox,
                new Label() {
                    Dock = DockStyle.Right,
                    Width = 100
                },
                btn_ToggleTab,
            });
            this.Controls.AddRange(new Control[] { brow_Transparent, browser, brow_ShortCutBar, toolbar, });
            f_brow_Go(brow_URL);
        }
        
        void f_brow_Go(string url)
        {
            url = url.Trim();
            if ((url.IndexOf(' ') == -1 && url.IndexOf('.') != -1) || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                browser.Navigate(url);
            }
            else
            {
                f_brow_Go("https://www.google.com.vn/search?q=" + HttpUtility.UrlEncode(url));
                //f_brow_Go("https://www.bing.com/search?q=" + HttpUtility.UrlEncode(url));
            }
        }

        void f_brow_onBeforeNavigating(Uri uri)
        {
            this.f_log("BeforeNavigating: ", uri);

            brow_URL = uri.ToString();
            brow_Domain = brow_URL.Split('/')[2];
            browser.Host = brow_Domain;

            brow_Transparent.BringToFront();
            brow_UrlTextBox.Text = brow_URL;
        }

        void f_brow_onDOMContentLoaded(string title, Uri uri)
        {
            this.f_log("DOMContentLoaded: ", uri);
            brow_Transparent.SendToBack();
            this.Text = title;
        }

        void f_brow_onConsoleMessage(string message) {
            //this.f_log("ConsoleMessage: ", message);
        }


        void f_brow_Close()
        {
            browser.Dispose();
            Xpcom.Shutdown();
        }

        #endregion

        #region [  === TAB === ]

        #region [ TAB: MAIN ]

        const string tab_IconToggle = "☰";
        FATabStrip tab_Main;
        FATabStripItem tab_Note;
        FATabStripItem tab_Link;
        FATabStripItem tab_Setting;

        void f_tab_Init()
        {
            tab_Main = new FATabStrip() { Dock = DockStyle.Right, Width = 399, AlwaysShowClose = false, AlwaysShowMenuGlyph = false };
            tab_Link = new FATabStripItem() { Title = "Link", CanClose = false };
            tab_Note = new FATabStripItem() { Title = "Note", CanClose = false };
            tab_Setting = new FATabStripItem() { Title = "Setting", CanClose = false };

            tab_Main.Items.AddRange(new FATabStripItem[] {
                tab_Link,
                tab_Note,
                tab_Setting,
            });

            this.Controls.AddRange(new Control[] {
                new Splitter() { Dock = DockStyle.Right, MinExtra = 0, MinSize = 0 },
                tab_Main,
            });

            f_tab_SettingInit();
            f_tab_LinkInit();
            f_tab_LinkNote();
        }

        void f_tab_Toggle()
        {
            if (tab_Main.Width != 0)
            {
                tab_Main.Tag = tab_Main.Width;
                tab_Main.Width = 0;
            }
            else
            {
                if (tab_Main.Tag != null)
                    tab_Main.Width = (int)tab_Main.Tag;
                else tab_Main.Width = 399;
            }
        }

        #endregion

        #region [ TAB:LINK ]

        TextBoxWaterMark tab_LinkSearchTextBox;
        System.Windows.Forms.TreeView tab_LinkTreeView;

        void f_tab_LinkInit()
        {
            Panel barSearch = new Panel()
            {
                Height = 23,
                Dock = DockStyle.Top,
                //BackColor = Color.Gray,
                Padding = new Padding(9, 1, 0, 0),
            };
            tab_LinkSearchTextBox = new TextBoxWaterMark()
            {
                WaterMark = "Search Link",
                Dock = DockStyle.Right,
                Height = 19,
                BorderStyle = BorderStyle.None,
                WaterMarkForeColor = Color.Gray,
                WaterMarkActiveForeColor = Color.DarkGray,
            };
            barSearch.Controls.AddRange(new Control[] {
                tab_LinkSearchTextBox
            });

            tab_LinkTreeView = new System.Windows.Forms.TreeView()
            {
                Dock = DockStyle.Fill,
                Font = font_Title,
                BorderStyle = BorderStyle.None,
            };

            Panel barFooter = new Panel()
            {
                Height = 24,
                Dock = DockStyle.Bottom,
                BackColor = Color.Gray,
            };

            tab_Link.Controls.AddRange(new Control[] {
                tab_LinkTreeView,
                barSearch,
                barFooter,
                new Label() { Dock = DockStyle.Left, Width = 1, BackColor = Color.LightGray }
            });
        }

        #endregion

        #region [ TAB:NOTE ]

        void f_tab_LinkNote()
        {

        }

        #endregion

        #region [ TAB: SETTING ]

        void f_tab_SettingInit()
        {

            var btn_Devtool = new Button() { Text = "Dev", Width = 45, Height = 20, Dock = DockStyle.Top };
            btn_Devtool.Click += (se, ev) =>
            {
                //browser.ShowDevTools();
            };

            var btn_EnableJS = new Button() { Text = "JS", Width = 45, Height = 20, Dock = DockStyle.Top, BackColor = Color.OrangeRed };
            btn_EnableJS.MouseClick += (se, ev) =>
            {
                brow_EnabelJS = brow_EnabelJS ? false : true;
                if (brow_EnabelJS)
                    btn_EnableJS.BackColor = Color.OrangeRed;
                else
                    btn_EnableJS.BackColor = SystemColors.Control;
            };
            var btn_EnableCSS = new Button() { Text = "CSS", Width = 45, Height = 20, Dock = DockStyle.Top };
            btn_EnableCSS.MouseClick += (se, ev) =>
            {
                brow_EnableCSS = brow_EnableCSS ? false : true;
                if (brow_EnableCSS)
                    btn_EnableCSS.BackColor = Color.OrangeRed;
                else
                    btn_EnableCSS.BackColor = SystemColors.Control;
            };
            var btn_EnableImg = new Button() { Text = "Image", Width = 45, Height = 20, Dock = DockStyle.Top, BackColor = Color.OrangeRed };
            btn_EnableImg.MouseClick += (se, ev) =>
            {
                brow_EnableImg = brow_EnableImg ? false : true;
                if (brow_EnableImg)
                    btn_EnableImg.BackColor = Color.OrangeRed;
                else
                    btn_EnableImg.BackColor = SystemColors.Control;
            };
            var btn_EnableAutoCache = new Button() { Text = "AutoRequest", Width = 79, Height = 20, Dock = DockStyle.Top };
            btn_EnableAutoCache.MouseClick += (se, ev) =>
            {
                brow_AutRequest = brow_AutRequest ? false : true;
                if (brow_AutRequest)
                    btn_EnableAutoCache.BackColor = Color.OrangeRed;
                else
                    btn_EnableAutoCache.BackColor = SystemColors.Control;
            };

            tab_Setting.Controls.AddRange(new Control[] {
                btn_Devtool , btn_EnableCSS, btn_EnableJS, btn_EnableImg, btn_EnableAutoCache,
            });
        }

        #endregion

        #endregion
    }

}
