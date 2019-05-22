using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.XPath;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Configuration;


namespace NowOnAir
{
    public partial class frmMain : Form
    {
        // default
        private string _feedUrl = "http://radiobox2.omroep.nl/data/radiobox2/nowonair/2.json";


        public frmMain()
        {
            InitializeComponent();

            // The ContextMenu property sets the menu that will
            // appear when the systray icon is right clicked.
            notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
        }


        private void frmMain_Load(object sender, EventArgs e)
        {
            Hide();
            string feedUrl = ConfigurationManager.AppSettings["FeedUrl"];
            if (feedUrl != null)
                _feedUrl = feedUrl;
            LoadData();
        }


        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // doubleclick to open window seemed like a nice feature, but it doesn't work very well
            //ShowWindow();
        }


        private void ShowWindow()
        {
            // hide balloontip
            notifyIcon1.Visible = false;
            //if (this.WindowState == FormWindowState.Minimized)
            //    this.WindowState = FormWindowState.Normal;
            this.Activate();
            Show();
            WindowState = FormWindowState.Normal;
            LoadData();
        }


        private void frmMain_Resize(object sender, EventArgs e)
        {
            // minimize to tray
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon1.Visible = true;
                Hide();
            }
        }


        private void LoadData()
        {
            // show status in textbox
            tbOutput.Text = "loading...";
            tbOutput.Update();

            try
            {
                NowPlaying np = LoadDataFromFreed();
                tbOutput.Text = JsonConvert.SerializeObject(np, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                tbOutput.Text = ex.Message;
            }

            btRefresh.Focus();
        }


        private NowPlaying LoadDataFromFreed()
        {
            // in office environments, often a proxy-server is used
            IWebProxy proxy = WebRequest.GetSystemWebProxy();
            proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

            WebClient wc = new WebClient();
            wc.Proxy = proxy;
            // we don't want any old (cached) data
            wc.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);

            string json = wc.DownloadString(_feedUrl);

            NowPlaying np = new NowPlaying();
            JsonConvert.PopulateObject(json, np);


            return np;
        }


        private void btRefresh_Click(object sender, EventArgs e)
        {
            tbOutput.Text = "loading...";
            tbOutput.Update();
            btRefresh.Enabled = false;
            LoadData();
            btRefresh.Enabled = true;
        }




        private void btAbout_Click(object sender, EventArgs e)
        {
            int year = DateTime.Now.Year;
            string about = String.Format("Copyright © 2008-{0} Joep Versleijen", year);

            tbOutput.Text = about;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            //ShowWindow();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowWindow();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    // TODO: vreemde tekens lijken nog niet geladen te kunnen worden.

                    // TODO: Animated icon weergeven tijdens laden. Zie:
                    //       http://www.daveamenta.com/2008-05/c-webclient-usage/


                    NowPlaying np = LoadDataFromFreed();

                    string artist = np.Results[0].songfile.artist;
                    string title = np.Results[0].songfile.title;

                    notifyIcon1.BalloonTipText = String.Format("{0} - {1}", artist, title);
                    notifyIcon1.ShowBalloonTip(1000);
                }
                catch
                {
                    notifyIcon1.BalloonTipText = "fout bij inlezen data";
                    notifyIcon1.ShowBalloonTip(10);
                }
            }
        }
    }
}