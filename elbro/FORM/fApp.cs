﻿using FarsiLibrary.Win;
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
using System.Linq;
using Gecko.DOM;
using System.Threading;
using System.Text.RegularExpressions;

namespace elbro
{
    public class fApp : fBase
    {
        const bool brow_AutoCacheFile = true;
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
                            if (m.Output.Ok)
                                if (m.Output.GetData() is oLink[])
                                    f_tabLink_drawNodes((oLink[])m.Output.GetData());
                            break;
                        case MESSAGE_ACTION.URL_REQUEST_CACHE:
                            break;
                    }
                    break;
            }
        }

        #region [ === FORM === ]

        public fApp(IJobAction jobAction) : base(jobAction)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Text = "English";
            this.OnReceiveMessage += f_event_OnReceiveMessage;
            this.Shown += f_form_Shown;
            this.FormClosing += f_form_Closing;

            f_brow_Init();
            f_tab_Init();
        }

        void f_form_Closing(object sender, FormClosingEventArgs e)
        {
            f_brow_Close();
        }

        void f_form_Shown(object sender, EventArgs e)
        {
            //f_brow_Go(brow_URL);
            brow_LinkOnPage.Location = new Point(this.Width - (tab_Main.Width + brow_LinkOnPage.Width), brow_UrlTextBox.Height);
            brow_LinkOnPage.Height = this.Height - brow_UrlTextBox.Height;

            f_tabLink_drawNodes(null);
        }
         
        #endregion

        #region [ === BROWSER === ]

        #region [ VAR ]

        //////☆★☐☑⧉✉⦿⦾⚠⚿⛑✕✓⥀✖↭☊⦧▷◻◼⟲≔☰⚒❯►❚❚❮⟳⚑⚐✎✛
        //////🕮🖎✍⦦☊🕭🔔🗣🗢🖳🎚🏷🖈🎗🏱🏲🗀🗁🕷🖒🖓👍👎♥♡♫♪♬♫🎙🎖🗝●◯⬤⚲☰⚒🕩🕪❯►❮⟳⚐🗑✎✛🗋🖫⛉ ⛊ ⛨⚏★☆
        ////const string tab_caption_store = "►";
        ////const string tab_caption_search = "⚲";
        ////const string tab_caption_word = "W";
        ////const string tab_caption_word_detail = "WD";
        //////const string tab_caption_word_detail = "⛉";
        ////const string tab_caption_listen = "?";
        ////const string tab_caption_pronunce = "P"; //☊
        ////const string tab_caption_writer = "✍";
        ////const string tab_caption_grammar = "Grammar";
        ////const string tab_caption_text = "Text";
        ////const string tab_caption_book = "Book";
        ////const string tab_caption_browser = "☰";

        const string BROW_BUTTON_TOGGLE_TAB = "☰";
        const string BROW_BUTTON_BACK = "❮";
        const string BROW_BUTTON_NEXT = "❯";

        const string DOMAIN_GOOGLE = "www.google.com.vn";
        const string DOMAIN_BING = "www.bing.com";
        const string DOMAIN_YOUTUBE = "www.youtube.com";
        const string DOMAIN_YOUTUBE_IMG = "i.ytimg.com";

        const int TOOLBAR_HEIGHT = 28;
        const int SHORTCUTBAR_HEIGHT = 17;

        //string brow_URL = "https://www.google.com.vn";
        //string brow_URL = "https://dictionary.cambridge.org";
        //string brow_URL = "https://www.codeproject.com/Articles/7933/Smart-Thread-Pool";
        //string brow_URL = "https://dictionary.cambridge.org/grammar/british-grammar/present-perfect-simple-i-have-worked";
        //string brow_URL = "https://dictionary.cambridge.org/grammar/british-grammar/do-or-make";
        string brow_URL = "https://en.oxforddictionaries.com/grammar/";
        //string brow_URL = "https://vietjack.com/";
        //string brow_URL = "https://vietjack.com/ngu-phap-tieng-anh/thi-hien-tai-tiep-dien-trong-tieng-anh.jsp";
        //string brow_URL = "https://hocmai.vn/khoa-hoc-truc-tuyen/1009/tieng-anh-5-10-nam-2018-2019.html";
        //string brow_URL = "https://hocmai.vn/khoa-hoc-truc-tuyen/644/hoc-tieng-anh-tu-con-so-0-thay-pham-trong-hieu.html";
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
        bool
            brow_IsReadCache = false,
            brow_EnabelJS = true,
            brow_EnableCSS = false,
            brow_EnableImg = false,
            brow_AutRequest = false;

        TextBoxWaterMark brow_UrlTextBox;
        GeckoWebBrowser browser;
        Panel brow_Transparent;
        Panel brow_ShortCutBar;

        //int brow_linkIndexHistory = 0;
        //List<string> brow_linkHistory = new List<string>();
        //DictionaryThreadSafe<string, string> brow_cacheResponse = new DictionaryThreadSafe<string, string>();

        int brow_linkBackIndexHistory = 0;
        List<string> brow_linkHistoryList = new List<string>();
        List<string> brow_linkReferentList = new List<string>();
        DictionaryThreadSafe<string, string> brow_cacheResponse = new DictionaryThreadSafe<string, string>();
        ListThreadSafe<string> brow_UrlFullRequest = new ListThreadSafe<string>();
        StringBuilder brow_CssBuilder = new StringBuilder();

        ListBox brow_LinkOnPage;
        Button brow_linkCloseButton;
        TextBoxWaterMark brow_linkRefSearchTextBox;

        #endregion

        void f_brow_Init()
        {
            #region [ browser ] 

            brow_Domain = brow_URL.Split('/')[2];
            browser = new GeckoWebBrowser();
            browser.BackColor = SystemColors.Control;
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

            //browser.AddMessageEventListener("myFunction", ((string msg) => {
            //    ;
            //}));
            //browser.NavigateFinishedNotifier.BlockUntilNavigationFinished();
            browser.UseHttpActivityObserver = true;
            browser.ObserveHttpModifyRequest += f_brow_ObserveHttpModifyRequest;
            //browser.ObserveHttpModifyRequest += (sender, e) => e.Channel.SetRequestHeader(name, value, merge: true);
            browser.DOMContentLoaded += (se, ev) => { GeckoWebBrowser w = (GeckoWebBrowser)se; if (w != null) f_brow_onDOMContentLoaded(w.DocumentTitle, w.Url); };
            browser.Navigating += (se, ev) => { f_brow_onBeforeNavigating(ev); };
            //browser.ConsoleMessage += (se, ev) => { f_brow_onConsoleMessage(ev.Message); };
            //browser.DocumentCompleted += (se, ev) => { f_brow_onDocumentCompleted();};
            //browser.DomDoubleClick += f_brow_onDomDoubleClick;
            browser.DomClick += f_brow_onDomClick;
            //browser.DomClick += f_brow_onDomClick2;

            browser.NoDefaultContextMenu = true;
            browser.ShowContextMenu += f_brow_onShowContextMenu;

            browser.DomKeyPress += f_brow_onDomKeyPress;
            //browser.JavascriptError += f_brow_onJavascriptError;

            browser.CreateControl();

            MyObserver oObs = new MyObserver();
            oObs.TicketLoadedEvent += (se, ev) => f_brow_cacheUpdate(ev.Url, ev.Data);
            ObserverService.AddObserver(oObs);

            #endregion

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

            brow_Transparent = new Panel()
            {
                BackColor = SystemColors.Control,
                Location = new Point(0, 0),
                Size = new Size(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom
            };
            brow_Transparent.MouseDoubleClick += (se, ev) => { brow_Transparent.SendToBack(); };

            Panel toolbar = new Panel() { Dock = DockStyle.Top, Height = TOOLBAR_HEIGHT, BackColor = SystemColors.Control, Padding = new Padding(3, 3, 0, 3) };
            brow_ShortCutBar = new Panel() { Dock = DockStyle.Top, Height = SHORTCUTBAR_HEIGHT, BackColor = SystemColors.Control, Padding = new Padding(0) };

            brow_LinkOnPage = new ListBox()
            {
                Font = font_Title,
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.None,
                Visible = false,
            };
            brow_linkRefSearchTextBox = new TextBoxWaterMark()
            {
                WaterMark = "Links search ...",
                Dock = DockStyle.Right,
                Width = 99,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center,
                WaterMarkForeColor = Color.DarkGray,
                BackColor = SystemColors.Control,
            };
            brow_linkCloseButton = new Button()
            {
                Text = "Close",
                Dock = DockStyle.Right,
                Width = 55,
                Visible = false
            };
            //brow_linkRefSearchTextBox.GotFocus += (se, ev) => {
            //    f_brow_linkOnPageSearch(brow_linkRefSearchTextBox.Text);
            //};
            brow_linkRefSearchTextBox.Click += (se, ev) =>
            {
                f_brow_linkOnPageSearch(brow_linkRefSearchTextBox.Text);
            };
            brow_linkRefSearchTextBox.DoubleClick += (se, ev) =>
            {
                f_brow_linkOnPageSearch(brow_linkRefSearchTextBox.Text);
            };

            brow_linkRefSearchTextBox.KeyDown += (se, ev) =>
            {
                if (ev.KeyCode == Keys.Enter)
                    f_brow_linkOnPageSearch(brow_linkRefSearchTextBox.Text);
            };
            brow_linkCloseButton.Click += (se, ev) => { f_brow_BrowserVisiable(); };
            brow_ShortCutBar.Controls.AddRange(new Control[] {
                brow_linkRefSearchTextBox,
                brow_linkCloseButton,
            });

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

            var btn_ToggleTab = new Button()
            {
                Text = BROW_BUTTON_TOGGLE_TAB,
                Width = 19,
                Height = 20,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.DarkGray,
                BackColor = SystemColors.Control
            };
            var btn_back = new Button()
            {
                Text = BROW_BUTTON_BACK,
                TextAlign = ContentAlignment.MiddleRight,
                Width = 32,
                Font = font_Title,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.DarkGray,
                BackColor = SystemColors.Control
            };
            var btn_next = new Button()
            {
                Text = BROW_BUTTON_NEXT,
                TextAlign = ContentAlignment.MiddleLeft,
                Width = 32,
                Font = font_Title,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.DarkGray,
                BackColor = SystemColors.Control
            };

            btn_ToggleTab.FlatAppearance.BorderColor = SystemColors.Control;
            btn_ToggleTab.MouseClick += (se, ev) => { f_tab_Toggle(); };
            brow_LinkOnPage.SelectedIndexChanged += (se, ev) =>
            {
                string url = "http" + brow_LinkOnPage.SelectedItem.ToString().Split(new string[] { " | http" }, StringSplitOptions.None)[1];

                //brow_Transparent.BringToFront();
                //browser.Visible = true;
                //brow_LinkOnPage.Visible = false;

                f_brow_Go(url);

                f_brow_BrowserVisiable();
            };

            btn_back.FlatAppearance.BorderColor = SystemColors.Control;
            btn_next.FlatAppearance.BorderColor = SystemColors.Control;
            btn_back.Click += (se, ev) => f_brow_goBack();
            btn_next.Click += (se, ev) => f_brow_goNext();

            toolbar.Controls.AddRange(new Control[] { brow_UrlTextBox,
                new Label() {
                    Dock = DockStyle.Right,
                    Width = 100
                },
                btn_back,
                btn_next,
                btn_ToggleTab,
            });
            this.Controls.AddRange(new Control[] {
                brow_Transparent,
                browser,
                brow_LinkOnPage,
                brow_ShortCutBar,
                toolbar,
            });
            f_brow_Go(brow_URL);
        }

        #region [ REQUEST ]

        bool f_requestCancel(string url, string refer)
        {
            //if (url.Contains(DOMAIN_YOUTUBE_IMG)) return false;

            if (refer.Length > 0 && refer != brow_URL)
                if (brow_UrlFullRequest.IndexOf(refer) != -1) return false;

            if (url.Contains("/chat/")) return true;

            if (brow_cacheResponse.ContainsKey(url)
                || (url.Contains(".js") && !url.EndsWith(".jsp")) || url.Contains("/js/")
                || url.Contains(brow_Domain) == false
                || url.Contains("about:")
                || url.Contains("json")
                || url.Contains("accounts.google.com")
                || url.Contains("/image") || url.Contains(".png") || url.Contains(".jpeg") || url.Contains(".jpg") || url.Contains(".gif")
                || url.Contains("font") || url.Contains(".svg") || url.Contains(".woff") || url.Contains(".ttf"))
                return true;

            return false;
        }

        private void f_brow_ObserveHttpModifyRequest(object sender, GeckoObserveHttpModifyRequestEventArgs e)
        {
            string url = e.Channel.Uri.ToString();
            string refer = e.Channel.Referrer == null ? "" : e.Channel.Referrer.ToString();

            bool cancel = f_requestCancel(url, refer);
            if (cancel)
            {
                //System.Tracer.WriteLine("---->[2] Observe REQUEST CANCEL: " + url);
                //httpChannel.Cancel(nsIHelperAppLauncherConstants.NS_BINDING_ABORTED);
                e.Cancel = true;
                return;
            }
            System.Tracer.WriteLine("---->[2] Observe REQUEST OK: " + url + Environment.NewLine + "REF: " + refer);
        }

        #endregion

        #region [ CACHE ]

        string f_brow_convertUrlToPathFileCache(string url)
        {
            return "cache/" + url.Split('?')[0].Split('#')[0].Substring(url.Split('/')[0].Length + 2).Replace(":", string.Empty).Replace("/", string.Empty) + ".htm";
        }

        void f_brow_cacheUpdate(string url, string data)
        {
            if (url.Contains(brow_Domain))
            {
                if (!string.IsNullOrEmpty(data))
                {
                    if (brow_cacheResponse.ContainsKey(url))
                        brow_cacheResponse[url] = data;
                    else
                    {
                        if (url == brow_URL && brow_AutoCacheFile)
                        {
                            brow_CssBuilder.Length = 0;
                            brow_CssBuilder.Capacity = 0;

                            string file = f_brow_convertUrlToPathFileCache(url);
                            if (!File.Exists(file))
                                File.WriteAllText(file, data);
                        }
                        else {
                            if (url.Contains(".css"))
                            {
                                brow_CssBuilder.Append(Environment.NewLine);
                                brow_CssBuilder.Append("/* " + url + " */");
                                brow_CssBuilder.Append(Environment.NewLine);
                                brow_CssBuilder.Append(data);
                                brow_CssBuilder.Append(Environment.NewLine);
                            }
                        }

                        if (url == brow_URL)
                            brow_cacheResponse.Add(url, data);
                    }
                }
            }
        }

        bool f_brow_cacheLoadPageHTML(string uri)
        {
            if (brow_cacheResponse.ContainsKey(uri))
            {
                string htm = brow_cacheResponse[uri];
                htm = f_brow_htmlFormat(htm);

                browser.Stop();
                browser.LoadHtml(htm);
                browser.NavigateFinishedNotifier.BlockUntilNavigationFinished();

                brow_Transparent.SendToBack();

                this.f_log("BeforeNavigating ===> CACHE: " + uri);
                return true;
            }
            return false;
        }

        #endregion

        #region [ GO, DOM_LOADED]

        void f_brow_Go(string url)
        {
            brow_Transparent.Width = browser.Width;
            brow_Transparent.BringToFront();

            url = url.Trim();
            this.Text = url;

            // online
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

        void f_brow_GoCache(string url)
        {
            brow_Transparent.BringToFront();
            string raw = brow_cacheResponse[url];
            if (raw != null)
            {
                string s = raw, title = string.Empty, htm = string.Empty;

                var mbody = Regex.Match(s, @"(?<=<body[^>]*>)[\s\S]*?(?=</body>)");
                if (mbody.Success) s = mbody.Value;
                s = Regex.Replace(s, @"<script[^>]*>[\s\S]*?</script>", string.Empty);

                var mtit = Regex.Match(raw, @"(?<=<title[^>]*>)[\s\S]*?(?=</title>)");
                if (mtit.Success) title = mtit.Value;
                this.Text = title;

                //s = f_brow_htmlFormat(s);
                htm = @"<!DOCTYPE html><html xmlns=""http://www.w3.org/1999/xhtml"" xml:lang=""en"" lang=""en""><head><meta http-equiv=""Content-Type"" content =""text /html; charset=UTF-8"" /><meta name=""viewport"" content=""width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no"" />" +
                "<title>" + title + "</title>" + f_brow_cssPublish(true) + @"</head><body><div id=""___links""></div>" + s + "</body></html>";

                brow_URL = url;
                brow_Domain = brow_URL.Split('/')[2];

                browser.Stop();
                browser.LoadHtml(htm);
                browser.NavigateFinishedNotifier.BlockUntilNavigationFinished();
                browser.Refresh();
                


                this.f_log("[END] LOAD_CACHE: " + url);

                TimerAction.SetTimeout(() =>
                {
                    browser.crossThreadPerformSafely(() => {

                    //var elink = browser.Document.GetElementById("___links");
                    //if (elink != null) {
                        //var h1s = browser.Document.GetElementsByTagName("h1");
                        //if (h1s.Length > 0) {
                        //    GeckoInputElement h1 = h1s[h1s.Length - 1] as GeckoInputElement;
                        
                        //}
                    //}

                    });
                    brow_Transparent.crossThreadPerformSafely(() =>
                    {
                        brow_Transparent.SendToBack();
                    });
                }, 100);
            }
        }

        void f_brow_GoYouTube(string url)
        {
            string urlEmbed = "https://www.youtube.com/embed/" + url.Split('=')[1];

            if (brow_UrlFullRequest.IndexOf(urlEmbed) == -1)
                brow_UrlFullRequest.Add(urlEmbed);

            Form f = new Form();
            f.Text = url;
            var w = new GeckoWebBrowser { Dock = DockStyle.Fill };
            w.DocumentCompleted += (se, ev) =>
            {
                f.Text = w.DocumentTitle;
            };
            f.Controls.Add(w);
            w.Navigate(urlEmbed);
            f.FormClosing += (se, ev) =>
            {
                w.Stop();
                w.Dispose();
                if (brow_UrlFullRequest.IndexOf(url) != -1)
                    brow_UrlFullRequest.Remove(url);
            };
            f.Show();
        }

        void f_brow_onBeforeNavigating(GeckoNavigatingEventArgs ev)
        {
            if (ev.Uri == null) return;

            string url = ev.Uri.ToString();
            this.f_log("[1] BeforeNavigating: " + url);
            brow_UrlTextBox.Text = url;

            // cache memory
            if (brow_cacheResponse.ContainsKey(url))
            {
                brow_IsReadCache = true;
                f_brow_GoCache(url);
                return;
            }

            // cache file
            string file = f_brow_convertUrlToPathFileCache(url);
            if (File.Exists(file))
            {
                string data = File.ReadAllText(file);
                brow_cacheResponse.Add(url, data);
                
                brow_IsReadCache = true;
                f_brow_GoCache(url);
                return;
            }

            // online
            brow_IsReadCache = false;

            brow_URL = url.ToString();
            brow_Domain = brow_URL.Split('/')[2];
        }

        void f_brow_onDOMContentLoaded(string title, Uri uri)
        {
            if (brow_IsReadCache) return;

            this.f_log("[2] DOMContentLoaded: " + uri.ToString());

            bool exist = brow_linkHistoryList.Where(x => x.EndsWith(brow_URL)).Count() > 0;
            if (exist == false)
            {
                brow_linkBackIndexHistory = 0;
                string key = browser.DocumentTitle.Trim().ToUpper() + " | " + brow_URL;
                brow_linkHistoryList.Insert(0, key);
            }

            browser.Document.Body.ScrollTop = 0;
            this.Text = title;

            f_brow_cssBinding();

            //GeckoElementCollection h1s = browser.Document.GetElementsByTagName("h1");
            //if (h1s.Length > 0)
            //{
            //    GeckoDivElement h1 = new GeckoHeadingElement(h1s[h1s.Length - 1].DomObject);
            //    h1.ScrollIntoView(true);
            //    //h1.ScrollTop += 10;
            //    int h1w = h1.OffsetLeft + h1.ClientWidth;
            //}

            f_brow_BrowserVisiable();

            /////////////////////////////////////////////////
            return;



            string li, href;
            GeckoElementCollection links = browser.Document.GetElementsByTagName("a");
            if (links.Length > 0)
            {
                for (int i = 0; i < links.Length; i++)
                {
                    href = links[i].OuterHtml.ToLower();
                    if (href.IndexOf("href") != -1)
                    {
                        href = href.Split(new string[] { "href" }, StringSplitOptions.None)[1].Trim();
                        if (href[0] == '=') href = href.Substring(1).Trim();
                        if (href[0] == '"') href = href.Substring(1).Trim();
                        href = href.Split('"')[0].Trim();
                        if (href.Contains("facebook") || href.Contains("google")) href = string.Empty;
                    }

                    if (href.Contains(brow_Domain) &&
                        !string.IsNullOrEmpty(links[i].TextContent) && links[i].TextContent.Trim().Length > 0
                        && !string.IsNullOrEmpty(href) && href.Length > 0 && href[0] != '#')
                    {
                        li = links[i].TextContent.Trim().ToUpper() + " | " + href;
                        if (brow_linkReferentList.IndexOf(li) == -1)
                            brow_linkReferentList.Add(li);
                    }
                }
            }

        }

        #endregion

        #region [ BACK, NEXT ]

        void f_brow_goBack()
        {
            if (brow_linkHistoryList.Count <= 1) return;

            brow_linkBackIndexHistory++;
            if (brow_linkBackIndexHistory == brow_linkHistoryList.Count || brow_linkBackIndexHistory < 0) brow_linkBackIndexHistory = 0;
            string url = "http" + brow_linkHistoryList[brow_linkBackIndexHistory].Split(new string[] { " | http" }, StringSplitOptions.None)[1];
            f_brow_Go(url);
        }

        void f_brow_goNext()
        {
            if (brow_linkHistoryList.Count <= 1) return;

            if (brow_linkBackIndexHistory == 0) brow_linkBackIndexHistory = brow_linkHistoryList.Count - 1;
            brow_linkBackIndexHistory--;
            if (brow_linkBackIndexHistory == brow_linkHistoryList.Count || brow_linkBackIndexHistory < 0) brow_linkBackIndexHistory = 0;
            string url = "http" + brow_linkHistoryList[brow_linkBackIndexHistory].Split(new string[] { " | http" }, StringSplitOptions.None)[1];
            f_brow_Go(url);
        }

        #endregion

        #region [ CSS, HTML ]

        const string brow_CSS_FIX =
        #region
@"
html *::before,html *::after,
i::before,i::after,
a::before,a::after,
li::before,li::after,
p::before,p::after,
div::before,div::after { content:"""" !important; }

li {list-style:none;}
table td { width:auto !important; }

svg, img, iframe, header, footer, nav,
input[type=""radio""], input[type=""check""], input[type=""submit""], input[type=""button""], 
textarea, select, button { display:none !important; }

.adsbygoogle { display:none !important; }

#___links{
position: absolute;
left: 0;
top: 0;
width: 100%;
height: 99px;
background-color: #292929;
color: #fff;
z-index: 99999;
}

";
        #endregion

        string f_brow_cssPublish(bool hasTagStyle)
        {
            string css = string.Empty, css_key = brow_Domain + ".css";

            //string css = string.Join(Environment.NewLine, brow_cacheResponse.Where(x => x.Key.Contains(brow_Domain) && x.Key.Contains(".css")).Select(x => x.Value).ToArray());
            if (brow_cacheResponse.ContainsKey(css_key))
                css = brow_cacheResponse[css_key];
            else {

                if (!brow_cacheResponse.ContainsKey(brow_Domain + ".css"))
                {
                    string file = "cache/" + brow_Domain + ".css";
                    if (File.Exists(file))
                    {
                        css = File.ReadAllText(file);
                        brow_cacheResponse.Add(css_key, css);
                    }

                    brow_CssBuilder.Length = 0;
                    brow_CssBuilder.Capacity = 0;
                }
                else
                { 
                    if (brow_CssBuilder.Length > 0)
                    {
                        css = brow_CssBuilder.ToString();
                        brow_cacheResponse.Add(css_key, css);

                        brow_CssBuilder.Length = 0;
                        brow_CssBuilder.Capacity = 0;

                        string file = "cache/" + css_key;
                        if (!File.Exists(file)) File.WriteAllText(file, css);
                    }
                }
            }

            css += brow_CSS_FIX;
            if (hasTagStyle)
                css = @"<style type=""text/css"">\r\n " + css + " \r\n</style>";
            return css;
        }

        string f_brow_htmlFormat(string s)
        {
            var mbody = Regex.Match(s, @"(?<=<body[^>]*>)[\s\S]*?(?=</body>)");
            if (mbody.Success) s = mbody.Value;

            s = Regex.Replace(s, @"<script[^>]*>[\s\S]*?</script>", string.Empty);
            s = Regex.Replace(s, @"<script[^>]*>[\s\S]*?</script>", string.Empty);
            //s = Regex.Replace(s, @"<style[^>]*>[\s\S]*?</style>", string.Empty);
            s = Regex.Replace(s, @"<noscript[^>]*>[\s\S]*?</noscript>", string.Empty);
            s = Regex.Replace(s, @"(?s)(?<=<!--).+?(?=-->)", string.Empty).Replace("<!---->", string.Empty);
            s = Regex.Replace(s, @"<form[^>]*>[\s\S]*?</form>", string.Empty);
            s = Regex.Replace(s, @"<select[^>]*>[\s\S]*?</select>", string.Empty);
            s = Regex.Replace(s, @"</?(?i:base|header|footer|nav|form|input|select|option|fieldset|button|iframe|link|symbol|path|canvas|use|ins|svg|embed|object|frameset|frame|meta)(.|\n|\s)*?>", string.Empty, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            //// Remove attribute style="padding:10px;..."
            //s = Regex.Replace(s, @"<([^>]*)(\sstyle="".+?""(\s|))(.*?)>", string.Empty);
            //s = s.Replace(@">"">", ">");

            string[] lines = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.None).Select(x => x.Trim()).Where(x => x.Length > 0).ToArray();
            s = string.Join(string.Empty, lines);
            return s;
        }

        void f_brow_cssBinding()
        {
            GeckoDocument doc = browser.Document;
            var head = doc.GetElementsByTagName("head").First();
            GeckoStyleElement css = doc.CreateElement("style") as GeckoStyleElement;
            css.Type = "text/css";
            css.TextContent = f_brow_cssPublish(false);
            head.AppendChild(css);
        }

        void f_brow_getCss()
        {
            GeckoDocument contentDocument = browser.Document;
            var userModifiedStyleSheet = contentDocument.StyleSheets.FirstOrDefault(s =>
            {
                // workaround for bug #40 (https://bitbucket.org/geckofx/geckofx-29.0/issue/40/xpath-error-hresult-0x805b0034)
                // var titleNode = s.OwnerNode.EvaluateXPath("@title").GetSingleNodeValue();
                var titleNode = s.OwnerNode.EvaluateXPath("@title").GetNodes().FirstOrDefault();
                if (titleNode == null)
                    return false;
                return titleNode.NodeValue == "userModifiedStyles";
            });

            if (userModifiedStyleSheet != null)
            {
                f_brow_SaveCustomizedCssRules(userModifiedStyleSheet);
            }

            //enhance: we have jscript for this: cleanup()... but running jscript in this method was leading the browser to show blank screen
            //              foreach (XmlElement j in _editDom.SafeSelectNodes("//div[contains(@class, 'ui-tooltip')]"))
            //              {
            //                  j.ParentNode.RemoveChild(j);
            //              }
            //              foreach (XmlAttribute j in _editDom.SafeSelectNodes("//@ariasecondary-describedby | //@aria-describedby"))
            //              {
            //                  j.OwnerElement.RemoveAttributeNode(j);
            //              }
        }

        void f_brow_SaveCustomizedCssRules(GeckoStyleSheet userModifiedStyleSheet)
        {
            try
            {
                /* why are we bothering to walk through the rules instead of just copying the html of the style tag? Because that doesn't
                 * actually get updated when the javascript edits the stylesheets of the page. Well, the <style> tag gets created, but
                 * rules don't show up inside of it. So
                 * this won't work: _editDom.GetElementsByTagName("head")[0].InnerText = userModifiedStyleSheet.OwnerNode.OuterHtml;
                 */
                var styles = new StringBuilder();
                styles.AppendLine("<style title='userModifiedStyles' type='text/css'>");
                foreach (var cssRule in userModifiedStyleSheet.CssRules)
                {
                    styles.AppendLine(cssRule.CssText);
                }
                styles.AppendLine("</style>");
                //////Debug.WriteLine("*User Modified Stylesheet in browser:" + styles);
                ////_pageEditDom.GetElementsByTagName("head")[0].InnerXml = styles.ToString();
            }
            catch (GeckoJavaScriptException jsex)
            {
                /* We are attempting to catch and ignore all JavaScript errors encountered here,
                 * specifically addEventListener errors and JSError (BL-279, BL-355, et al.).
                 */
                //Logger.WriteEvent("GeckoJavaScriptException (" + jsex.Message + "). We're swallowing it but listing it here in the log.");
                //Debug.Fail("GeckoJavaScriptException(" + jsex.Message + "). In Release version, this would not show.");
            }
        }

        void f_event_CssRulesAddAfterDocumentCompleted()
        {
            // We want to suppress several of the buttons that the control normally shows.
            // It's nice if we don't have to modify the html and related files, because they are unzipped from a package we install
            // from a source I'm not sure we control, and installed into a directory we can't modify at runtime.
            // A workaround is to tweak the stylesheet to hide them. The actual buttons (and two menu items) are easily
            // hidden by ID.
            // Unfortunately we're getting rid of a complete group in the pull-down menu, which leaves an ugly pair of
            // adjacent separators. And the separators don't have IDs so we can't easily select just one to hide.
            // Fortunately there are no other divs in the parent (besides the separator) so we just hide the second one.
            // This is unfortunately rather fragile and may not do exactly what we want if the viewer.html file
            // defining the pdfjs viewer changes.
            GeckoStyleSheet stylesheet = browser.Document.StyleSheets.First();
            stylesheet.CssRules.Add("#toolbarViewerRight, #viewOutline, #viewAttachments, #viewThumbnail, #viewFind {display: none}");
            stylesheet.CssRules.Add("#previous, #next, #pageNumberLabel, #pageNumber, #numPages {display: none}");
            stylesheet.CssRules.Add("#toolbarViewerLeft .splitToolbarButtonSeparator {display: none}");
        }

        #endregion

        #region [ MENU_CONTEXT ]

        //browser.NoDefaultContextMenu = true;
        //browser.ShowContextMenu += f_brow_onShowContextMenu;

        void f_brow_onShowContextMenu(object sender, GeckoContextMenuEventArgs e)
        {
            var mn_ReadMode = new MenuItem("Read Mode", f_brow_menuReadModeClick, Shortcut.F12) { };
            e.ContextMenu.MenuItems.Add(0, mn_ReadMode);

            var mn_OpenOnChrome = new MenuItem("Open on Browser", f_brow_menuOpenOnBrowserClick) { };
            e.ContextMenu.MenuItems.Add(0, mn_OpenOnChrome);

            #region
            //////            MenuItem FFMenuItem = null;
            //////            Debug.Assert(!InvokeRequired);
            //////            if (ContextMenuProvider != null)
            //////            {
            //////                var replacesStdMenu = ContextMenuProvider(e);
            //////                var mn_ReadMode = new MenuItem("Read Mode", f_brow_menuReadModeClick, Shortcut.F12) { };
            //////                e.ContextMenu.MenuItems.Add(0, mn_ReadMode);



            //////#if DEBUG
            //////                FFMenuItem = AddOpenPageInFFItem(e);
            //////#endif

            //////                if (replacesStdMenu)
            //////                    return; // only the provider's items
            //////            }
            //////            var m = e.ContextMenu.MenuItems.Add("Menu test ...");
            //////            m.Enabled = !string.IsNullOrEmpty(GetPathToStylizer());

            //////            if (FFMenuItem == null)
            //////                AddOpenPageInFFItem(e);
            //////#if DEBUG
            //////            AddOtherMenuItemsForDebugging(e);
            //////#endif

            //e.ContextMenu.MenuItems.Add(LocalizationManager.GetString("Browser.CopyTroubleshootingInfo", "Copy Troubleshooting Information"), OnGetTroubleShootingInformation);
            #endregion
        }

        private void f_brow_menuOpenOnBrowserClick(object sender, EventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "chrome.exe";
            process.StartInfo.Arguments = brow_URL;
            process.Start();
        }

        private void f_brow_menuReadModeClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// This Function will be passed a GeckoContextMenuEventArgs to which appropriate menu items
        /// can be added. If it returns true these are in place of our standard extensions; if false, the
        /// standard ones will follow whatever it adds.
        /// </summary>
        public Func<GeckoContextMenuEventArgs, bool> ContextMenuProvider
        {
            get; set;
        }

        private void AddOtherMenuItemsForDebugging(GeckoContextMenuEventArgs e)
        {
            //e.ContextMenu.MenuItems.Add("Open about:memory window", OnOpenAboutMemory);
            //e.ContextMenu.MenuItems.Add("Open about:config window", OnOpenAboutConfig);
            //e.ContextMenu.MenuItems.Add("Open about:cache window", OnOpenAboutCache);
        }
        private MenuItem AddOpenPageInFFItem(GeckoContextMenuEventArgs e)
        {
            //return e.ContextMenu.MenuItems.Add(
            //    LocalizationManager.GetString("Browser.OpenPageInFirefox", "Open Page in Firefox (which must be in the PATH environment variable)"),
            //    OnOpenPageInSystemBrowser);
            return e.ContextMenu.MenuItems.Add("Menu test 1");
        }
        public static string GetPathToStylizer()
        {
            return "";// FileLocator.LocateInProgramFiles("Stylizer.exe", false, new string[] { "Skybound Stylizer 5" });
        }

        #endregion

        #region [ SHOW, HIDE ]

        private void f_brow_linkRefVisiable()
        {
            brow_Transparent.SendToBack();

            browser.Visible = false;
            brow_LinkOnPage.Visible = true;
            brow_linkCloseButton.Visible = true;

            brow_LinkOnPage.Focus();
        }

        private void f_brow_BrowserVisiable()
        {
            brow_Transparent.SendToBack();

            browser.Visible = true;
            brow_linkCloseButton.Visible = false;
            brow_LinkOnPage.Visible = false;
        }

        #endregion

        #region [ SEARCH LINK ]

        private void f_brow_linkOnPageSearch(string text)
        {
            text = text.ToLower().Trim();
            brow_LinkOnPage.Items.Clear();

            //string[] a1 = link_listHistory.Where(x => x.ToLower().Contains(text)).OrderBy(x => x).ToArray();
            //for (int i = 0; i < a1.Length; i++) brow_LinkOnPage.Items.Add(a1[i]);

            for (int i = 0; i < brow_linkHistoryList.Count; i++) brow_LinkOnPage.Items.Add(brow_linkHistoryList[i]);

            string[] a2 = brow_linkReferentList.Where(x => x.ToLower().Contains(text)).OrderBy(x => x).ToArray();
            for (int i = 0; i < a2.Length; i++) brow_LinkOnPage.Items.Add(a2[i]);

            f_brow_linkRefVisiable();
        }

        #endregion

        #region [ DOM_CLICK ]

        void f_brow_onDomClick(object sender, DomMouseEventArgs eventArgs)
        {
            try
            {
                this.f_log("DOM___CLICK ... ... ...");

                GeckoAnchorElement anchor;
                var inode = eventArgs.Target.NativeObject;
                GeckoElement el = new GeckoElement(inode);
                if (el.TagName == "A" || el.ParentNode.NodeName == "A")
                {
                    if (el.TagName == "A")
                        anchor = new GeckoAnchorElement(el.DomObject);
                    else
                        anchor = new GeckoAnchorElement(el.ParentElement.DomObject);

                    if (string.IsNullOrEmpty(anchor.Href)) return;

                    string url = anchor.Href.Trim();
                    if (!url.StartsWith("http"))
                    {
                        if (url.StartsWith("../"))
                        {
                            url = url.Substring(3);
                            string split = url.Split('/')[0];
                            if (split == url)
                            {
                                //[1] is page-abc.html
                                string[] a = brow_URL.Split('/'); // 0/1/2/../..(.)html
                                if (a[a.Length - 1].Contains("."))
                                {
                                    //[1.1] path ref exist (.)html
                                    if (a.Length > 5)
                                    {
                                        // exist middle path /../
                                        url = brow_URL.Substring(0, brow_URL.Length - (a[a.Length - 2].Length + a[a.Length - 1].Length + 1)) + url;
                                    }
                                    else
                                    {
                                        // not exist middle path /../
                                        url = brow_URL.Substring(0, brow_URL.Length - a[a.Length - 1].Length) + url;
                                    }
                                }
                                else
                                {
                                    //[1.2] path ref not exist (.)html
                                    if (a.Length > 5)
                                    {
                                        // exist middle path /../
                                        url = brow_URL.Substring(0, brow_URL.Length - (a[a.Length - 2].Length + a[a.Length - 1].Length + 1)) + url;
                                    }
                                    else
                                    {
                                        // not exist middle path /../
                                        url = brow_URL.Substring(0, brow_URL.Length - a[a.Length - 1].Length) + url;
                                    }
                                }
                            }
                            else
                            {
                                //[2] is path-xyz/.../page-abc.html
                                url = brow_URL.Split(new string[] { split }, StringSplitOptions.None)[0] + url;
                            }
                        }
                    }

                    //https://www.google.com.vn/url?q=https://www.britishcouncilfoundation.id/en/english&sa=U&ved=0ahUKEwiUmbaSxYncAhWFo5QKHYPGC4Y4ChAWCEAwCQ&usg=AOvVaw0xa_Ri84llhzTgWLE29d57
                    if (url.Contains(brow_Domain) && url.Contains("/url?q="))
                        url = url.Split(new string[] { "/url?q=", "&" }, StringSplitOptions.None)[1].Trim();
                    url = HttpUtility.UrlDecode(url);
                    string domain = url.IndexOf("//") == -1 ? url.Split('/')[0] : url.Split('/')[2];
                    switch (domain)
                    {
                        case DOMAIN_YOUTUBE:
                            f_brow_GoYouTube(url);
                            break;
                        default:
                            f_brow_Go(url);
                            break;
                    }
                    eventArgs.Handled = true; // cancel
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR DOM_CLICK: " + ex.Message);
            }
        }

        public event EventHandler OnBrowserClick;
        void f_brow_onDomClick2(object sender, DomEventArgs e)
        {
            Debug.Assert(!InvokeRequired);
            //this helps with a weird condition: make a new page, click in the text box, go over to another program, click in the box again.
            //it loses its focus.
            browser.WebBrowserFocus.Activate();//trying to help the disappearing cursor problem

            EventHandler handler = OnBrowserClick;
            if (handler != null)
                handler(this, e);
        }

        /// <summary>
        /// When you receive a OnBrowserClick and have determined that nothing was clicked on that the c# needs to pay attention to,
        /// pass it on to this method. It will either let the browser handle it normally, or redirect it to the operating system
        /// so that it can open the file or external website itself.
        /// </summary>
        public void HandleLinkClick(GeckoAnchorElement anchor, DomEventArgs eventArgs, string workingDirectoryForFileLinks)
        {
            Debug.Assert(!InvokeRequired);
            if (anchor.Href.ToLowerInvariant().StartsWith("http")) //will cover https also
            {
                Process.Start(anchor.Href);
                eventArgs.Handled = true;
                return;
            }
            if (anchor.Href.ToLowerInvariant().StartsWith("file"))
            //links to files are handled externally if we can tell they aren't html/javascript related
            {
                // TODO: at this point spaces in the file name will cause the link to fail.
                // That seems to be a problem in the DomEventArgs.Target.CastToGeckoElement() method.
                var href = anchor.Href;

                var path = href.Replace("file:///", "");

                if (new List<string>(new[] { ".pdf", ".odt", ".doc", ".docx", ".txt" }).Contains(Path.GetExtension(path).ToLowerInvariant()))
                {
                    eventArgs.Handled = true;
                    Process.Start(new ProcessStartInfo()
                    {
                        FileName = path,
                        WorkingDirectory = workingDirectoryForFileLinks
                    });
                    return;
                }
                eventArgs.Handled = false; //let gecko handle it
                return;
            }
            else if (anchor.Href.ToLowerInvariant().StartsWith("mailto"))
            {
                eventArgs.Handled = true;
                Process.Start(anchor.Href); //let the system open the email program
                Debug.WriteLine("Opening email program " + anchor.Href);
            }
            else
            {
                //ErrorReport.NotifyUserOfProblem("Bloom did not understand this link: " + anchor.Href);
                eventArgs.Handled = true;
            }
        }

        private void f_brow_onDomClick3(object sender, DomMouseEventArgs e)
        {
            GeckoRange range = browser.Window.Selection.GetRangeAt(0);
            if (range.StartOffset != range.EndOffset)
            {
                string selectedText = range.CloneContents().TextContent.Trim();
                MessageBox.Show(selectedText);
            }
        }

        private void f_brow_onDomDoubleClick(object sender, DomMouseEventArgs e)
        {
            ////GeckoElement scriptEl = browser.Document.CreateElement("script");
            ////scriptEl.TextContent = brow_JS;
            ////GeckoNode res = browser.Document.Head.AppendChild(scriptEl);
            ////using (var java = new AutoJSContext(browser.Window.JSContext))
            ////{
            ////    JsVal result = java.EvaluateScript("fireEvent()", browser.Window.DomWindow);
            ////    MessageBox.Show(result.ToString());
            ////}

            //// case 01
            //string JSresult = "";
            //bool bExec;
            //using (AutoJSContext JScontext = new AutoJSContext(browser.Window.JSContext))
            //    bExec = JScontext.EvaluateScript("window.getSelection().toString();", (nsISupports)browser.Window.DomWindow, out JSresult);

            // case 02
            GeckoRange range = browser.Window.Selection.GetRangeAt(0);
            if (range.StartOffset != range.EndOffset)
            {
                string selectedText = range.CloneContents().TextContent.Trim();
                MessageBox.Show(selectedText);
            }
        }

        #endregion

        #region [ ... ]

        public string f_brow_javaScript_Run(string script)
        {
            Debug.Assert(!InvokeRequired);
            // Review JohnT: does this require integration with the NavigationIsolator?
            if (browser.Window != null) // BL-2313 two Alt-F4s in a row while changing a folder name can do this
            {
                using (var context = new AutoJSContext(browser.Window))
                {
                    string result;
                    context.EvaluateScript(script, (nsISupports)browser.Document.DomObject, out result);
                    return result;
                }
            }
            return null;
        }

        public void f_brow_saveHTML(string path)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(f_brow_saveHTML), path);
                return;
            }
            browser.SaveDocument(path, "text/html");
        }

        public void f_brow_javaScript_InsertDOM(string script_text)
        {
            //Debug.Assert(!InvokeRequired);
            GeckoDocument doc = browser.Document;
            var head = doc.GetElementsByTagName("head").First();
            GeckoScriptElement script = doc.CreateElement("script") as GeckoScriptElement;
            script.Type = "text/javascript";
            script.Text = script_text;
            head.AppendChild(script);
        }

        /// <summary>
        /// Workaround suggested by Tom Hindle, since GeckoFx-45's CanXSelection properties aren't working.
        /// </summary>
        /// <returns></returns>
        public bool f_brow_IsThereACurrentTextSelection()
        {
            using (var win = new GeckoWindow(browser.WebBrowserFocus.GetFocusedWindowAttribute()))
            {
                var sel = win.Selection;
                if (sel.IsCollapsed || sel.FocusNode is GeckoImageElement)
                    return false;
            }
            return true;
        }

        public void f_brow_Copy()
        {
            Debug.Assert(!InvokeRequired);
            browser.CopySelection();
        }

        private void f_brow_Paste()
        {
            // Saved as an example of how to do a special paste. But since we introduced modules,
            // if we want this we have to get the ts code into the FrameExports system.
            //if (Control.ModifierKeys == Keys.Control)
            //{
            //  var text = PortableClipboard.GetText(TextDataFormat.UnicodeText);
            //  text = System.Web.HttpUtility.JavaScriptStringEncode(text);
            //  RunJavaScript("BloomField.CalledByCSharp_SpecialPaste('" + text + "')");
            //}
            //else
            //{
            //just let ckeditor do the MSWord filtering
            browser.Paste();
            //}
        }

        /// <summary>
        /// Prevent a CTRL+V pasting when we have the Paste button disabled, e.g. when pictures are on the clipboard.
        /// Also handle CTRL+N creating a new page on Linux/Mono.
        /// </summary>
        void f_brow_onDomKeyPress(object sender, DomKeyEventArgs e)
        {
            //Debug.Assert(!InvokeRequired);
            //const uint DOM_VK_INSERT = 0x2D;

            ////enhance: it's possible that, with the introduction of ckeditor, we don't need to pay any attention
            ////to ctrl+v. I'm doing a hotfix to a beta here so I don't want to change more than necessary.
            //if ((e.CtrlKey && e.KeyChar == 'v') || (e.ShiftKey && e.KeyCode == DOM_VK_INSERT)) //someone was using shift-insert to do the paste
            //{
            //    if (_pasteCommand == null /*happened in calendar config*/ || !_pasteCommand.Enabled)
            //    {
            //        Debug.WriteLine("Paste not enabled, so ignoring.");
            //        e.PreventDefault();
            //    }
            //    //otherwise, ckeditor will handle the paste
            //}
            //// On Windows, Form.ProcessCmdKey (intercepted in Shell) seems to get ctrl messages even when the browser
            //// has focus.  But on Mono, it doesn't.  So we just do the same thing as that Shell.ProcessCmdKey function
            //// does, which is to raise this event.
            //if (SIL.PlatformUtilities.Platform.IsMono && ControlKeyEvent != null && e.CtrlKey && e.KeyChar == 'n')
            //{
            //    Keys keyData = Keys.Control | Keys.N;
            //    ControlKeyEvent.Raise(keyData);
            //}
        }

        private void f_brow_onJavascriptError(object sender, JavascriptErrorEventArgs e)
        {
            if (e.Message.Contains("sourceMapping"))
                return;
            //var file = e.Filename.Split(new char[] { '/' }).Last();
            //var line = (int)e.Line;
            //var dir = FileLocator.GetDirectoryDistributedWithApplication(BloomFileLocator.BrowserRoot);
            //var mapPath = Path.Combine(dir, file + ".map");
            //if (RobustFile.Exists(mapPath))
            //{
            //    var consumer = new SourceMapDotNet.SourceMapConsumer(RobustFile.ReadAllText(mapPath));
            //    foreach (var match in consumer.OriginalPositionsFor(line))
            //    {
            //        file = match.File;
            //        line = match.LineNumber;
            //        break;
            //    }
            //}
            //Debug.WriteLine("{0} in {1}:{2}", e.Message, file, line);
            //NonFatalProblem.Report(ModalIf.None, PassiveIf.All, e.Message,
            //    string.Format("{0} in {1}:{2}", e.Message, file, line));
        }

        void f_brow_onConsoleMessage(string message)
        {
            if (message != null && message.Length > 0 && message.IndexOf('$') != -1)
                this.f_log(message);
        }

        #endregion

        void f_brow_Close()
        {
            brow_UrlFullRequest.Clear();
            brow_linkHistoryList.Clear();
            brow_cacheResponse.Clear();

            browser.Dispose();
            Xpcom.Shutdown();
        }

        #endregion

        #region [  === TAB === ]

        #region [ TAB: MAIN ]

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
            f_tabLink_Init();
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

        TextBoxWaterMark tabLink_SearchTextBox;
        System.Windows.Forms.TreeView tabLink_TreeView;

        void f_tabLink_Init()
        {
            Panel barSearch = new Panel()
            {
                Height = 23,
                Dock = DockStyle.Top,
                //BackColor = Color.Gray,
                Padding = new Padding(9, 1, 0, 0),
            };
            tabLink_SearchTextBox = new TextBoxWaterMark()
            {
                WaterMark = "Search Link",
                Dock = DockStyle.Right,
                Height = 19,
                BorderStyle = BorderStyle.None,
                WaterMarkForeColor = Color.Gray,
                WaterMarkActiveForeColor = Color.DarkGray,
            };
            barSearch.Controls.AddRange(new Control[] {
                tabLink_SearchTextBox
            });
            tabLink_SearchTextBox.KeyDown += (se, ev) =>
            {
                if (ev.KeyCode == Keys.Enter)
                    this.f_sendRequestToJob(JOB_NAME.SYS_LINK, MESSAGE_ACTION.ITEM_SEARCH, tabLink_SearchTextBox.Text.Trim());
            };

            tabLink_TreeView = new System.Windows.Forms.TreeView()
            {
                Visible = false,
                Dock = DockStyle.Fill,
                Font = font_Title,
                BorderStyle = BorderStyle.None,
            };
            tabLink_TreeView.MouseDoubleClick += f_tabLink_selectIndexChange;
            

            Panel barFooter = new Panel()
            {
                Height = 24,
                Dock = DockStyle.Bottom,
                BackColor = Color.Gray,
            };

            tab_Link.Controls.AddRange(new Control[] {
                tabLink_TreeView,
                barSearch,
                barFooter,
                new Label() { Dock = DockStyle.Left, Width = 1, BackColor = Color.LightGray }
            });
        }

        void f_tabLink_drawNodes(oLink[] links)
        {
            if (links != null && links.Length > 0)
            {
                List<string> tags = new List<string>();
                foreach (string[] a in links.Select(x => x.Tags.Split(','))) tags.AddRange(a);
                tags = tags.Select(x => x.Trim()).Distinct().ToList();
                TreeNode[] nodes = tags.Select(x => new TreeNode(x)).ToArray();
                foreach (TreeNode node in nodes) node.Nodes.AddRange(links.Where(o => o.Tags.Contains(node.Text)).Select(o => new TreeNode(o.TitleDomain()) { Tag = o }).ToArray());
                tabLink_TreeView.crossThreadPerformSafely(() =>
                {
                    tabLink_TreeView.Nodes.Clear();
                    tabLink_TreeView.Nodes.AddRange(new TreeNode[] {
                        new TreeNode("Youtube"){ Tag = new oLink(){ Title = "Youtube", Tags= "", Link = "https://www.youtube.com/results?search_query={0}" } },
                        new TreeNode("Google"){ Tag = new oLink(){ Title = "Google", Tags= "", Link = "https://www.google.com/search?q={0}" } },
                        new TreeNode("Grammar By Oxford"){ Tag = new oLink(){ Title = "Grammar By Oxford", Tags= "", Link = "https://en.oxforddictionaries.com/grammar/" } },
                        new TreeNode("British Grammar By Cambridge"){ Tag = new oLink(){ Title = "British Grammar By Cambridge", Tags= "", Link = "https://dictionary.cambridge.org/grammar/british-grammar/" } },
                        new TreeNode("Pronuncian.com"){ Tag = new oLink(){ Title = "Pronuncian.com", Tags= "", Link = "https://pronuncian.com/pronounce-th-sounds/" } },
                        new TreeNode("Learning-english-online.net"){ Tag = new oLink(){ Title = "Learning-english-online.net", Tags= "", Link = "https://www.learning-english-online.net/pronunciation/the-english-th/" } },
                    });
                    tabLink_TreeView.Nodes.AddRange(nodes);
                });
            }
            else
            {
                //tabLink_TreeView.crossThreadPerformSafely(() =>
                //{
                    tabLink_TreeView.Nodes.Clear();
                    tabLink_TreeView.Nodes.AddRange(new TreeNode[] {
                        new TreeNode("Youtube"){ Tag = new oLink(){ Title = "Youtube", Tags= "", Link = "https://www.youtube.com/results?search_query={0}" } },
                        new TreeNode("Google"){ Tag = new oLink(){ Title = "Google", Tags= "", Link = "https://www.google.com/search?q={0}" } },
                        new TreeNode("Grammar By Oxford"){ Tag = new oLink(){ Title = "Grammar By Oxford", Tags= "", Link = "https://en.oxforddictionaries.com/grammar/" } },
                        new TreeNode("British Grammar By Cambridge"){ Tag = new oLink(){ Title = "British Grammar By Cambridge", Tags= "", Link = "https://dictionary.cambridge.org/grammar/british-grammar/" } },
                        new TreeNode("Pronuncian.com"){ Tag = new oLink(){ Title = "Pronuncian.com", Tags= "", Link = "https://pronuncian.com/pronounce-th-sounds/" } },
                        new TreeNode("Learning-english-online.net"){ Tag = new oLink(){ Title = "Learning-english-online.net", Tags= "", Link = "https://www.learning-english-online.net/pronunciation/the-english-th/" } },
                    });
                //});

                TimerAction.SetTimeout(() =>
                {
                    tabLink_TreeView.crossThreadPerformSafely(() =>
                    {
                        tabLink_TreeView.Visible = true;
                    });
                }, 500);

                //tabLink_TreeView.Visible = true;
            }
        }

        void f_tabLink_selectIndexChange(object sender, EventArgs e)
        {
            TreeNode node = tabLink_TreeView.SelectedNode;
            if (node != null && node.Tag != null)
            {
                oLink link = node.Tag as oLink;
                string url = string.Empty;
                if (link.Title == "Youtube" || link.Title == "Google")
                {
                    string key = Prompt.ShowDialog("Input to search?", link.Title).Trim();
                    if (key.Length > 0)
                        url = string.Format(link.Link, key);
                }
                else url = link.Link;
                if (url.Length > 0)
                {
                    f_brow_Go(url);

                    //m_url_textBox.Text = url;
                    //m_tab_Browser.Text = link.TitleDomain();
                    //m_brow_web.DocumentText = "<h1>LOADING: " + url + "</h1>";

                    //this.f_sendRequestToJob(JOB_NAME.SYS_LINK, MESSAGE_ACTION.URL_REQUEST_CACHE, url);
                }
            }
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

        #region [ HISTORY ]


        #endregion
        #endregion
    }

}
