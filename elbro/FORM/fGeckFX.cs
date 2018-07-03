using System;
using System.Windows.Forms;
using Gecko;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace elbro
{
    class fGeckFX : Form
    {
        private TabControl m_tabControl;

        public fGeckFX()
        {
            this.Width = 800;
            this.Height = 600;

            m_tabControl = new TabControl();
            m_tabControl.Dock = DockStyle.Fill;

            AddTab();

            Controls.Add(m_tabControl);

            m_tabControl.ControlRemoved += delegate
            {
                if (m_tabControl.TabCount == 1)
                {
                    AddTab();
                }
            };

        }

        protected void ModifyElements(GeckoElement element, string tagName, Action<GeckoElement> mod)
        {
            while (element != null)
            {
                if (element.TagName == tagName)
                {
                    mod(element);
                }
                ModifyElements(element.FirstChild as GeckoHtmlElement, tagName, mod);
                element = (element.NextSibling as GeckoHtmlElement);
            }
        }

        protected void TestModifyingDom(GeckoWebBrowser browser)
        {
            GeckoElement g = browser.Document.DocumentElement;
            ModifyElements(g, "BODY", e =>
                              {
                                  for (int i = 1; i < 4; ++i)
                                  {
                                      var newElement = g.OwnerDocument.CreateElement(String.Format("h{0}", i));
                                      newElement.TextContent = "Geckofx added this text.";
                                      g.InsertBefore(newElement, e);
                                  }
                              });
        }

        protected GeckoWebBrowser AddTab()
        {
            var tabPage = new TabPage();
            tabPage.Text = "blank";
            var browser = new GeckoWebBrowser();
            browser.Dock = DockStyle.Fill;
            tabPage.DockPadding.Top = 25;
            tabPage.Dock = DockStyle.Fill;

            browser.UseHttpActivityObserver = true;
            //browser.ObserveHttpModifyRequest += (sender, e) => e.Channel.SetRequestHeader(name, value, merge: true);
            browser.ObserveHttpModifyRequest += Browser_ObserveHttpModifyRequest;


            // add a handler showing how to view the DOM
            //			browser.DocumentCompleted += (s, e) => 	TestQueryingOfDom(browser);

            // add a handler showing how to modify the DOM.
            //			browser.DocumentCompleted += (s, e) => TestModifyingDom(browser);

            // add a handle to detect when user modifies a contentEditable part of the document.
            //browser.DomInput += (sender, args) => MessageBox.Show(String.Format("User modified element {0}", (args.Target.CastToGeckoElement() as GeckoHtmlElement).OuterHtml));		    

            AddToolbarAndBrowserToTab(tabPage, browser);

            m_tabControl.TabPages.Add(tabPage);
            tabPage.Show();
            m_tabControl.SelectedTab = tabPage;

            browser.Navigate("https://dictionary.cambridge.org/");

            // Uncomment this to stop links from navigating.
            // browser.DomClick += StopLinksNavigating;

            // Demo use of ReadyStateChange.
            // For some special page, e.g. about:config browser.Document is null.
            browser.ReadyStateChange += (s, e) => this.Text = browser.Document != null ? browser.Document.ReadyState : "";

            browser.DocumentTitleChanged += (s, e) => tabPage.Text = browser.DocumentTitle;

            browser.EnableDefaultFullscreen();

            // Popup window management.
            browser.CreateWindow += (s, e) =>
            {
                // A naive popup blocker, demonstrating popup cancelling.
                Console.WriteLine("A popup is trying to show: " + e.Uri);
                if (e.Uri.StartsWith("http://annoying-site.com"))
                {
                    e.Cancel = true;
                    Console.WriteLine("A popup is blocked: " + e.Uri);
                    return;
                }

                // For <a target="_blank"> and window.open() without specs(3rd param),
                // e.Flags == GeckoWindowFlags.All, and we load it in a new tab;
                // otherwise, load it in a popup window, which is maximized by default.
                // This simulates firefox's behavior.
                if (e.Flags == GeckoWindowFlags.All)
                    e.WebBrowser = AddTab();
                else
                {
                    var wa = System.Windows.Forms.Screen.GetWorkingArea(this);
                    e.InitialWidth = wa.Width;
                    e.InitialHeight = wa.Height;
                }
            };

            return browser;
        }

        private void Browser_ObserveHttpModifyRequest(object sender, GeckoObserveHttpModifyRequestEventArgs e)
        {
            //e.Channel.SetRequestHeader(name, value, merge: true);
        }


        protected void AddToolbarAndBrowserToTab(TabPage tabPage, GeckoWebBrowser browser)
        {
            TextBox urlbox = new TextBox();
            urlbox.Top = 0;
            urlbox.Width = 200;

            Button nav = new Button();
            nav.Text = "Go";
            nav.Left = urlbox.Width;

            Button newTab = new Button();
            newTab.Text = "NewTab";
            newTab.Left = nav.Left + nav.Width;

            Button stop = new Button
            {
                Text = "Stop",
                Left = newTab.Left + newTab.Width
            };

            Button closeTab = new Button();
            closeTab.Text = "GC.Collect";
            closeTab.Left = stop.Left + stop.Width;

            Button closeWithDisposeTab = new Button();
            closeWithDisposeTab.Text = "Close";
            closeWithDisposeTab.Left = closeTab.Left + closeTab.Width;

            Button open = new Button();
            open.Text = "FileOpen";
            open.Left = closeWithDisposeTab.Left + closeWithDisposeTab.Width;

            Button print = new Button();
            print.Text = "Print";
            print.Left = open.Left + open.Width;

            Button scrollDown = new Button { Text = "Down", Left = closeWithDisposeTab.Left + 250 };
            Button scrollUp = new Button { Text = "Up", Left = closeWithDisposeTab.Left + 330 };

            scrollDown.Click += (s, e) => { browser.Window.ScrollByPages(1); };
            scrollUp.Click += (s, e) => { browser.Window.ScrollByPages(-1); };

            nav.Click += delegate
            {
                // use javascript to warn if url box is empty.
                if (string.IsNullOrEmpty(urlbox.Text.Trim()))
                    browser.Navigate("javascript:alert('hey try typing a url!');");
                browser.Navigate(urlbox.Text);
            };

            newTab.Click += delegate { AddTab(); };

            stop.Click += delegate { browser.Stop(); };

            closeTab.Click += delegate
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            };

            closeWithDisposeTab.Click += delegate
            {
                m_tabControl.Controls.Remove(tabPage);
                tabPage.Dispose();
            };

            open.Click += (s, a) =>
            {
                nsIFilePicker filePicker = Xpcom.CreateInstance<nsIFilePicker>("@mozilla.org/filepicker;1");
                filePicker.Init(browser.Window.DomWindow, new nsAString("hello"), nsIFilePickerConsts.modeOpen);
                //filePicker.AppendFilter(new nsAString("png"), new nsAString("*.png"));
                //filePicker.AppendFilter(new nsAString("html"), new nsAString("*.html"));
                if (nsIFilePickerConsts.returnOK == filePicker.Show())
                {
                    using (nsACString str = new nsACString())
                    {
                        filePicker.GetFileAttribute().GetNativePathAttribute(str);
                        browser.Navigate(str.ToString());
                    }
                }
            };
            //url in Navigating event may be the mapped version,
            //e.g. about:config in Navigating event is jar:file:///<xulrunner>/omni.ja!/chrome/toolkit/content/global/config.xul
            browser.Navigating += (s, e) =>
            {
                Console.WriteLine("Navigating: url: " + e.Uri + ", top: " + e.DomWindowTopLevel);
            };
            browser.Navigated += (s, e) =>
            {
                if (e.DomWindowTopLevel)
                    urlbox.Text = e.Uri.ToString();
                Console.WriteLine("Navigated: url: " + e.Uri + ", top: " + e.DomWindowTopLevel, ", errorPage: " + e.IsErrorPage);
            };

            browser.Retargeted += (s, e) =>
            {
                var ch = e.Request as Gecko.Net.Channel;
                Console.WriteLine("Retargeted: url: " + e.Uri + ", contentType: " + ch.ContentType + ", top: " + e.DomWindowTopLevel);
            };
            browser.DocumentCompleted += (s, e) =>
            {
                Console.WriteLine("DocumentCompleted: url: " + e.Uri + ", top: " + e.IsTopLevel);
            };

            print.Click += delegate { browser.Window.Print(); };

            tabPage.Controls.Add(urlbox);
            tabPage.Controls.Add(nav);
            tabPage.Controls.Add(newTab);
            tabPage.Controls.Add(stop);
            tabPage.Controls.Add(closeTab);
            tabPage.Controls.Add(closeWithDisposeTab);
            tabPage.Controls.Add(open);
            tabPage.Controls.Add(print);
            tabPage.Controls.Add(browser);
            tabPage.Controls.Add(scrollDown);
            tabPage.Controls.Add(scrollUp);
        }
    }
}

