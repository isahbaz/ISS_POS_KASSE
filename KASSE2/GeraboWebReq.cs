using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace IS_KASSE
{
    public class GeraboWebReq
    {
        private string _url;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }
        string _servername;

        public string Servername
        {
            get { return _servername; }
            set { _servername = value; }
        }
        public GeraboWebReq()
        {
            this.Url = Servername;
        }
        public GeraboPunkteeinlosung RedeemPremium(string ApiKey, string GeraboCode, string credits, string premid, string premEAN)
        {
            try
            {
                string url = this.Url + "redeempremium?apikey=" + ApiKey + "&code=" + GeraboCode + "&credits=" + credits;

                // Create a request for the URL.   
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.  
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                GeraboPunkteeinlosung root = new GeraboPunkteeinlosung();
                root = JsonConvert.DeserializeObject<GeraboPunkteeinlosung>(responseFromServer, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();
                return root;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
        public GeraboPunkteeinlosung CreditEinlosen(string ApiKey, string GeraboCode, string credits)
        {
            try
            {
                string url = this.Url + "redeemcredits?apikey=" + ApiKey + "&code=" + GeraboCode + "&credits=" + credits;

                // Create a request for the URL.   
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.  
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                GeraboPunkteeinlosung root = new GeraboPunkteeinlosung();
                root = JsonConvert.DeserializeObject<GeraboPunkteeinlosung>(responseFromServer, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();
                return root;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
        public GeraboPunkteeinlosung PunkteZumGeld(string ApiKey, string GeraboCode)
        {
            try
            {
                string url = this.Url + "redeempremium?apikey=" + ApiKey + "&code=" + GeraboCode;

                // Create a request for the URL.   
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.  
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                GeraboPunkteeinlosung root = new GeraboPunkteeinlosung();
                root = JsonConvert.DeserializeObject<GeraboPunkteeinlosung>(responseFromServer, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();
                return root;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
        public GeraboAccountInfo GeraboKundenInfo(string ApiKey, string GeraboCode)
        {
            try
            {
                string url = this.Url + "getCodeData?apikey=" + ApiKey + "&code=" + GeraboCode;

                // Create a request for the URL.   
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.  
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                GeraboAccountInfo root= new GeraboAccountInfo();
                root = JsonConvert.DeserializeObject<GeraboAccountInfo>(responseFromServer, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();
                return root;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
        
        public GeraboRoot GeraboCollectPunte(string ApiKey, string GeraboCode, string Betrag)
        {
            try
            {
                string url = this.Url+"collectpoints?apikey=" + ApiKey + "&code=" + GeraboCode + "&volume=" + Betrag;

                // Create a request for the URL.   
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.  
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                GeraboRoot rootObject = JsonConvert.DeserializeObject<GeraboRoot>(responseFromServer, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();
                return rootObject;
            }
            catch(Exception ee)
            {
                return null;
            }
        }
        public GeraboPunkteeinlosung GeraboCollectCredit(string ApiKey, string GeraboCode, string Credit)
        {
            try
            {
                string url = this.Url + "collectcredits?apikey=" + ApiKey + "&code=" + GeraboCode + "&credits=" + Credit;

                // Create a request for the URL.   
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.  
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                GeraboPunkteeinlosung rootObject = null;
                rootObject= JsonConvert.DeserializeObject<GeraboPunkteeinlosung>(responseFromServer, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();
                return rootObject;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
        public GeraboPunkteeinlosung GeraboCashback(string ApiKey, string GeraboCode, string Point)
        {
            try
            {
                Int32 punkte=0;
                string url = "";
                if(Int32.TryParse(Point,out punkte))
                {
                }
                if (punkte == 0)
                {
                    url = this.Url + "cashback?apikey=" + ApiKey + "&code=" + GeraboCode ;
                }
                else
                {
                    url = this.Url + "cashback?apikey=" + ApiKey + "&code=" + GeraboCode + "&credits=" + Point;
                }
                // Create a request for the URL.   
                WebRequest request = WebRequest.Create(url);
                // If required by the server, set the credentials.  
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                // Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                GeraboPunkteeinlosung rootObject = null;
                rootObject = JsonConvert.DeserializeObject<GeraboPunkteeinlosung>(responseFromServer, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                //Console.WriteLine(responseFromServer);
                // Clean up the streams and the response.  
                reader.Close();
                response.Close();
                return rootObject;
            }
            catch (Exception ee)
            {
                return null;
            }
        }
    }
}
