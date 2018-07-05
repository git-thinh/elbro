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
using System.Linq;
using Gecko.DOM;
using System.Threading;

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
            brow_LinkOnPage.Location = new Point(this.Width - (tab_Main.Width + brow_LinkOnPage.Width), brow_UrlTextBox.Height);
            brow_LinkOnPage.Height = this.Height - brow_UrlTextBox.Height;
        }

        #endregion

        #region [ === BROWSER === ]

        #region [ VARIABLE ]

        const string DOMAIN_GOOGLE = "www.google.com.vn";
        const string DOMAIN_BING = "www.bing.com";
        const string DOM_CONTENT_LOADED = "DOM_CONTENT_LOADED";
        const int TOOLBAR_HEIGHT = 28;
        const int SHORTCUTBAR_HEIGHT = 17;

        //string brow_URL = "https://www.google.com.vn";
        //string brow_URL = "https://dictionary.cambridge.org";
        //string brow_URL = "https://dictionary.cambridge.org/grammar/british-grammar/present-perfect-simple-i-have-worked";
        //string brow_URL = "https://dictionary.cambridge.org/grammar/british-grammar/do-or-make";
        //string brow_URL = "https://en.oxforddictionaries.com/grammar/";
        //string brow_URL = "https://vietjack.com/";
        //string brow_URL = "https://vietjack.com/ngu-phap-tieng-anh/thi-hien-tai-tiep-dien-trong-tieng-anh.jsp";
        //string brow_URL = "https://hocmai.vn/khoa-hoc-truc-tuyen/1009/tieng-anh-5-10-nam-2018-2019.html";
        string brow_URL = "https://hocmai.vn/khoa-hoc-truc-tuyen/644/hoc-tieng-anh-tu-con-so-0-thay-pham-trong-hieu.html";
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
        bool brow_EnabelJS = true,
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

        ListBox brow_LinkOnPage;
        Button brow_linkCloseButton;
        TextBoxWaterMark brow_linkRefSearchTextBox;

        #endregion

        void f_brow_Init()
        {
            #region [ browser ] 

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
                Location = new Point(0, brow_UrlTextBox.Height),
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

            var btn_ToggleTab = new Button() { Text = tab_IconToggle, Width = 19, Height = 20, Dock = DockStyle.Right };
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

            var btn_back = new Button() { Text = "<", Width = 32, Dock = DockStyle.Right };
            var btn_next = new Button() { Text = ">", Width = 32, Dock = DockStyle.Right };
            btn_back.Click += (se, ev) =>
            {
                brow_linkBackIndexHistory++;
                if (brow_linkBackIndexHistory == brow_linkHistoryList.Count || brow_linkBackIndexHistory == -1) brow_linkBackIndexHistory = 0;
                string url = "http" + brow_linkHistoryList[brow_linkBackIndexHistory].Split(new string[] { " | http" }, StringSplitOptions.None)[1];
                f_brow_Go(url);
            };
            btn_next.Click += (se, ev) =>
            {
                if (brow_linkBackIndexHistory == 0) brow_linkBackIndexHistory = brow_linkHistoryList.Count - 1;
                brow_linkBackIndexHistory--;
                if (brow_linkBackIndexHistory == brow_linkHistoryList.Count || brow_linkBackIndexHistory == -1) brow_linkBackIndexHistory = 0;
                string url = "http" + brow_linkHistoryList[brow_linkBackIndexHistory].Split(new string[] { " | http" }, StringSplitOptions.None)[1];
                f_brow_Go(url);
            };

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

        bool f_requestCancel(string url)
        {
            //if (url.Contains("/chat/")) return true;

            ////if (
            ////    (url.Contains("play") && url.Contains(".js")) 
            ////    || (url.Contains("video") && url.Contains(".js"))
            ////    ) return false;

            //if (brow_cacheResponse.ContainsKey(url)
            //    || (url.Contains(".js") && !url.EndsWith(".jsp")) || url.Contains("/js/")
            //    || url.Contains(brow_Domain) == false
            //    || url.Contains("about:")
            //    || url.Contains("font") || url.Contains(".svg") || url.Contains(".woff") || url.Contains(".ttf")
            //    || url.Contains("/image") || url.Contains(".png") || url.Contains(".jpeg") || url.Contains(".jpg") || url.Contains(".gif"))
            //    return true;

            return false;
        }

        private void f_brow_ObserveHttpModifyRequest(object sender, GeckoObserveHttpModifyRequestEventArgs e)
        {
            string url = e.Channel.Uri.ToString();
            bool cancel = f_requestCancel(url);
            if (cancel)
            {
                //System.Tracer.WriteLine("---->[2] Observe REQUEST CANCEL: " + url);
                //httpChannel.Cancel(nsIHelperAppLauncherConstants.NS_BINDING_ABORTED);
                e.Cancel = true;
                return;
            }
            System.Tracer.WriteLine("---->[2] Observe REQUEST OK: " + url);

        }

        #endregion

        #region [ CACHE ]

        private void f_brow_cacheUpdate(string url, string data)
        {
            if (url.Contains(brow_Domain))
            {
                if (!string.IsNullOrEmpty(data))
                {
                    if (brow_cacheResponse.ContainsKey(url))
                        brow_cacheResponse.Add(url, data);
                    else
                        brow_cacheResponse[url] = data;
                }
            }
        }

        bool f_brow_cacheLoadPageHTML(string uri)
        {
            if (brow_cacheResponse.ContainsKey(uri))
            {
                string htm = brow_cacheResponse[uri];

                browser.Stop();
                browser.LoadHtml(htm);
                browser.NavigateFinishedNotifier.BlockUntilNavigationFinished();

                this.f_log("BeforeNavigating ===> CACHE: " + uri);
                return true;
            }
            return false;
        }


        #endregion

        #region [ CSS ]

        const string brow_CSS =
        #region
@"\r\n 
html, body, div, span, object, iframe,
h1, h2, h3, h4, h5, h6, p, blockquote, pre,
abbr, address, cite, code,
del, dfn, em, img, ins, kbd, q, samp,
small, strong, sub, sup, var,
b, i,
dl, dt, dd, ol, ul, li,
fieldset, form, label, legend,
table, caption, tbody, tfoot, thead, tr, th, td,
article, aside, canvas, details, figcaption, figure, 
footer, header, hgroup, menu, nav, section, summary,
time, mark, audio, video {
    margin:0;
    padding:0;
    border:0;
    outline:0;
    font-size:100%;
    vertical-align:baseline;
    background:transparent;
}

body {
    line-height:1;
}

article,aside,details,figcaption,figure,
footer,header,hgroup,menu,nav,section { 
    display:block;
}

nav ul {
    list-style:none;
}

blockquote, q {
    quotes:none;
}

blockquote:before, blockquote:after,
q:before, q:after {
    content:'';
    content:none;
}

a {
    margin:0;
    padding:0;
    font-size:100%;
    vertical-align:baseline;
    background:transparent;
}

ins {
    background-color:#ff9;
    color:#000;
    text-decoration:none;
}

mark {
    background-color:#ff9;
    color:#000; 
    font-style:italic;
    font-weight:bold;
}

del {
    text-decoration: line-through;
}

abbr[title], dfn[title] {
    border-bottom:1px dotted;
    cursor:help;
}

table {
    border-collapse:collapse;
    border-spacing:0;
}

/* change border colour to suit your needs */
hr {
    display:block;
    height:1px;
    border:0;   
    border-top:1px solid #cccccc;
    margin:1em 0;
    padding:0;
}

input, select {
    vertical-align:middle;
}

html *::before,html *::after,
i::before,i::after,
a::before,a::after,
li::before,li::after,
p::before,p::after,
div::before,div::after { content:"""" !important; }

table td { width:auto !important; }

iframe { display:none !important; }
img { display:none !important; }
form, input, textarea, select, button { display:none !important; }

.adsbygoogle { display:none !important; }

\r\n";
        #endregion

        void f_brow_cssBinding()
        {
            string css_text = string.Join(Environment.NewLine, brow_cacheResponse.Where(x => x.Key.Contains(brow_Domain) && x.Key.Contains(".css")).Select(x => x.Value).ToArray());
            css_text = "\r\n " + brow_CSS + css_text + " \r\n";

            GeckoDocument doc = browser.Document;
            var head = doc.GetElementsByTagName("head").First();
            GeckoStyleElement css = doc.CreateElement("style") as GeckoStyleElement;
            css.Type = "text/css";
            css.TextContent = css_text;
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

        #region [ GO, NAVIGATE, DOM_LOADED]

        void f_brow_onBeforeNavigating(GeckoNavigatingEventArgs ev)
        {
            brow_Transparent.Width = browser.Width;
            brow_Transparent.BringToFront();

            string url = ev.Uri.ToString();
            this.f_log("[1] BeforeNavigating: " + url);

            brow_URL = url.ToString();
            brow_Domain = brow_URL.Split('/')[2];
            brow_UrlTextBox.Text = brow_URL;

            if (f_brow_cacheLoadPageHTML(url))
            {
                ev.Cancel = true;
                f_brow_onDOMContentLoaded(browser.DocumentTitle, ev.Uri);
            }
        }

        void f_brow_onDOMContentLoaded(string title, Uri uri)
        {
            browser.Document.Body.ScrollTop = 0;
            this.f_log("[2] DOMContentLoaded: " + brow_URL);
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

            bool exist = brow_linkHistoryList.Where(x => x.EndsWith(brow_URL)).Count() > 0;
            if (exist == false)
            {
                brow_linkBackIndexHistory = 0;
                li = browser.DocumentTitle.Trim().ToUpper() + " | " + brow_URL;
                brow_linkHistoryList.Insert(0, li);
            }
        }

        void f_brow_Go(string url)
        {
            this.Text = url;

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

        #endregion

        #region [ DOM_CLICK ]

        void f_brow_onDomClick(object sender, DomMouseEventArgs eventArgs)
        {
            GeckoElement el = new GeckoElement(eventArgs.Target.NativeObject);
            if(el.TagName == "A")
            {
                GeckoAnchorElement anchor = new GeckoAnchorElement(el.DomObject);
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
                                else {
                                    // not exist middle path /../
                                    url = brow_URL.Substring(0, brow_URL.Length - a[a.Length - 1].Length) + url;
                                }
                            }
                            else {
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
                        else {
                            //[2] is path-xyz/.../page-abc.html
                            url = brow_URL.Split(new string[] { split }, StringSplitOptions.None)[0] + url;
                        }
                    }
                }
                f_brow_Go(url);
                eventArgs.Handled = true; // cancel
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
            brow_linkHistoryList.Clear();
            brow_cacheResponse.Clear();

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
