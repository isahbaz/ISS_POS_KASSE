using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;



namespace IS_KASSE
{
    public partial class F_Datev_Login : Form
    {
        public string codeChallenge = "", codeVerifier = "", Code = "", id_token = "", state = "", session_state = "", error = "", errorState = "", Uri_state = "", UUID = "";
        public Logger log = null;
        private void btnTastatur_Click(object sender, EventArgs e)
        {
            try
            {
                string Windir = "", OskDir = "";
                Windir = Environment.GetEnvironmentVariable("windir");
                Process.Start(@"c:\Windows\Sysnative\cmd.exe", "/c osk.exe");
                Thread.Sleep(1000);
                foreach (var process in Process.GetProcessesByName("cmd"))
                {
                    process.Kill();
                }

                /*
                 //System.Diagnostics.Process.Start("osk.exe");
                 OskDir = Windir + @"\System32\osk.exe";
                 System.Diagnostics.Process.Start(@"C:\Windows\System32\osk.exe");
                 string progFiles = @"C:\Program Files\Common Files\Microsoft Shared\ink";
                 string keyboardPath = Path.Combine(progFiles, "TabTip.exe");

                  Process.Start(keyboardPath);*/
            }
            catch (Exception xx)
            {
                System.Diagnostics.Process.Start(@"C:\Windows\System32\osk.exe");
                // MessageBox.Show(xx.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void F_Datev_Login_Load_1(object sender, EventArgs e)
        {
            button1.Enabled = false;
          
        }

        public F_Datev_Login()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private void F_Datev_Login_Load(object sender, EventArgs e)
        {

        }
        async void InitializeAsync()
        {
            log = new Logger("DATEV\\LOGIN\\");
            log.Log("InitializeAsync()");
            await webView.EnsureCoreWebView2Async(null);
            string URL = "https://meinfiskal.de/openid/authorize?state=" + Uri_state + "&scope=openid%20business:karo:cashregisterimport&response_type=code%20id_token&redirect_uri=http://localhost:8080&client_id=0949d36f-e40c-41b3-84c1-3312f6d461e8&nonce=" + UUID + "&code_challenge=" + this.codeChallenge + "&code_challenge_method=S256";
            log.Log("InitializeAsync() URL:"+URL);
            await webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync("window.chrome.webview.postMessage(window.document.URL);");
            webView.CoreWebView2.Navigate(URL);
            webView.CoreWebView2.WebMessageReceived += UpdateAddressBar;

        }

        void UpdateAddressBar(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {

            string[] returnedUri;
            String uri = args.TryGetWebMessageAsString();
            string Source = "";
            //MessageBox.Show(args.Source);
            Source = args.Source;
            if (Source.Contains("#code="))
            {

                returnedUri = Source.Split('&');
                if (returnedUri.Length == 4)
                {
                    Code = returnedUri[0].Split('=')[1];
                    id_token = returnedUri[1].Split('=')[1];
                    state = returnedUri[2].Split('=')[1];
                    session_state = returnedUri[3].Split('=')[1];
                    this.Close();
                }
            }
            else if (Source.Contains("#error="))
            {
                returnedUri = Source.Split('&');
                error = returnedUri[0].Split('=')[1];
                errorState = returnedUri[2];
                this.Close();

            }
            else
            {
                button1.Enabled = true;
            }
            
            //MessageBox.Show(uri);
            // addressBar.Text = uri;
            /* webView.CoreWebView2.PostWebMessageAsString(uri);
         http://localhost:8080/#code=MzljYTlkNDMtYzdmZC00Mjk2LTkxNzktOTMzMzU1NWQyODhmO2RTQnlsdFdDYWhKMHVxYStwVDVLOG43R0ljcXJFNjIvd1RQckZ4UDZxTU09&
             id_token =eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6ImZRLVV5amc3UXZUcGZfbW45eUtYbWVZUHNQRSJ9.eyJpc3MiOiJodHRwczovL2xvZ2luLmRhdGV2LmRlL29wZW5pZCIsInN1YiI6Ik13ZmZyaFN2VXF4azdlMWNzVzA5T1JCT1lCMDlTTndVRUFqcmxES2pjbW89IiwiYXVkIjoiMDk0OWQzNmYtZTQwYy00MWIzLTg0YzEtMzMxMmY2ZDQ2MWU4IiwiZXhwIjoxNjk3MjI5Njg4LCJpYXQiOjE2OTcyMjg3ODgsIm5iZiI6MTY5NzIyODc4OCwiY19oYXNoIjoidjIyZGRGdERENmY2WkVUSHpoWjdFUSIsIm5vbmNlIjoiMmNiMWVjMDAtNjkzNi0xMWVlLThjOTktMDI0MmFjMTIwMDAyIiwiYW1yIjpbInB3ZCJdfQ.OL5LbiNOk45RIfvkcKUyUUMBtOB3yMGSkGUwwN8T6Qft4N2xbTM9DNhpzll0-2lD-NmjU6RFvt4b2kBIL6xcyyL7hnjDDX6TF53lmZ_QBF-H4O3tGU4VMJXmbdlFoIIjgnBR1MjGUwuQI_RearsBKTK8O_yemnU5uozfeUkGABhgE8OZ3jPEQMipFLC2mWee9oXmVgFDE2ltbxnvfW7WySPT9hsJCx3qodrCjDZ-KONAI3kYRmaPVvNV0vYnqrhW-pLs_4FIb8H1fDpHJC2F8aXBee0MzQ3nUEeBF1P_XHKGOH4Mp4c8wuqkkpZ5woGFQH_wupDzMtRjUA8N2HXz06ARgO9RcK0bGpDa3byKNb16wfIVJsY0zc55khDQ0lqcxQN4DNuTuzYJz7N2TsZ1H4vZXVIvfCChGxSlyWEjRy4M09C5woJFgMK0XgjO_dPmnVqOlcHxUiIcSoIVZ6sPvm-YyD4kwnvHcCy9UZA_nuK65KS88lo0dBWWQtLWuOW8Qn_RmetaZizFm8-OoOmGj275VAvMo0ZBUorARD8xD27L9wmC5clI2wyHu3yTbwgqyfMhGNUQlyQf1c05nHxL9Qzs2GoM_X_L9ZFV-9aWPtujuktZF6whxHfR5hitlHmu29wo8y1lYOVZx8gewP9dvuyTFCepOVbaTKtINir8ckU&state=27371705933085254852&session_state=h4_ppKUf5iwZO5m8jlF_VCm8lA5VXw3zc8i8Qw8L6bE.KAYYdfiYB9YoY7ngFPgVZYVMk5f4PTZG
             */
        }
    }

}
