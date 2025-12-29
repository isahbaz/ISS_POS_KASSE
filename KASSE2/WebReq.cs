using System;
//using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace IS_KASSE
{
    class WebReq
    {
        //http://192.168.178.50/spar
        string serverName = "http://www.iss-pos.de/iss/flash/isskasse.php";

        public string[] anmelden(string kundenid, String Kundenname, String Stadt, String tel, string ip)
        {
            try
            {
                string[] data = new string[10];
                WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
                webreq.Method = "POST";
                string postData = "kundenid=" + kundenid + "&islem=1&kundenname=" + Kundenname + "&stadt=" + Stadt + "&tel=" + tel + "&ip=" + ip;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                webreq.ContentType = "application/x-www-form-urlencoded";
                webreq.ContentLength = byteArray.Length;
                Stream dataStream = webreq.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = webreq.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                data = responseFromServer.Split('#');
                return data;
            }
            catch (Exception ww)
            {

                //string msql = ww.Message;
                return null;

            }
        }
        public List<string> fragen(string kundenid)
        {
            //string grubad=
            string[] data = new string[10];
            List<string> Datareturn = new List<string>();
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "kundenid=" + kundenid + "&islem=2" ;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;
            // Clean up the streams.

            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split('#');
            //MessageBox.Show(responseFromServer);
            foreach (string ss in data)
            {
                Datareturn.Add(ss);
            }
            //List<string> gelenData = new List<string>(data); 

            return Datareturn;


        }
        public void ReturnValeu(string kundenid) //aktuel islemin uygulndiktan sonra aktuelligi ortadan kardirma
        {

            string[] data = new string[10];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "kundenid=" + kundenid+ "&islem=21";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split(',');
            //List<string> gelenData = new List<string>(data); 
           
        }
        public string[] webreqArtInsert(string barkod, string yAlisfiyat, string ySatisfiyat, string Yname, string Ygrup, string Ybirim, string Yagirlik, string Yrafmiktar, string Ydepomiktar, string hedefip, string serverip)
        {

            string[] data = new string[10];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "barcode=" + barkod + "&islem=4" + "&yalisfiyat=" + yAlisfiyat + "&ysatisfiyat=" + ySatisfiyat + "&yname=" + Yname + "&ygrup=" + Ygrup + "&ybirim=" + Ybirim + "&yagirlik=" + Yagirlik + "&yrafmiktar=" + Yrafmiktar + "&ydepomiktar=" + Ydepomiktar + "&hedefip=" + hedefip + "&serverip=" + serverip;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split(',');
            //List<string> gelenData = new List<string>(data); 
            return data;
        }
        public string[] GrupLoad()
        {

            string[] data = new string[50];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "islem=6";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded;encoding='iso-8859-1'";
            //webreq.Headers = "charset= iso-8859-1";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            //Encoding encoding;
            //encoding = Encoding.GetEncoding("iso-8859-1");
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split(',');
            //List<string> gelenData = new List<string>(data); 
            return data;
        }
        public string[] MarkaLoad()
        {

            string[] data = new string[50];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "islem=5";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded;encoding='iso-8859-1'";
            //webreq.Headers = "charset= iso-8859-1";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            //Encoding encoding;
            //encoding = Encoding.GetEncoding("iso-8859-1");
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split(',');
            //List<string> gelenData = new List<string>(data); 
            return data;
        }
        public string[] LifLoad()
        {

            string[] data = new string[50];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "islem=9";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded;encoding='iso-8859-1'";
            //webreq.Headers = "charset= iso-8859-1";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            //Encoding encoding;
            //encoding = Encoding.GetEncoding("iso-8859-1");
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split(',');
            //List<string> gelenData = new List<string>(data); 
            return data;
        }

        public string[] PfandLoad()
        {

            string[] data = new string[50];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "islem=10";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded;encoding='iso-8859-1'";
            //webreq.Headers = "charset= iso-8859-1";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            //Encoding encoding;
            //encoding = Encoding.GetEncoding("iso-8859-1");
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split('#');
            //List<string> gelenData = new List<string>(data); 
            return data;
        }
        public string[] Siparis(string kartno)
        {

            string[] data = new string[10];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "barcode=" + kartno + "&islem=7";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split('#');
            //List<string> gelenData = new List<string>(data); 
            return data;
        }
        public string[] Etiket(string kartno)
        {

            string[] data = new string[10];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "barcode=" + kartno + "&islem=8";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split('#');
            //List<string> gelenData = new List<string>(data); 
            return data;
        }
        public string[] cop(string barkod, string menge)
        {

            string[] data = new string[50];
            WebRequest webreq = (HttpWebRequest)WebRequest.Create(serverName);
            webreq.Method = "POST";
            string postData = "islem=11&barcode=" + barkod + "&menge=" + menge;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            webreq.ContentType = "application/x-www-form-urlencoded;encoding='iso-8859-1'";
            //webreq.Headers = "charset= iso-8859-1";
            webreq.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = webreq.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = webreq.GetResponse();
            // Display the status.
            //label1.Text = ((HttpWebResponse)response).StatusDescription;
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            //Encoding encoding;
            //encoding = Encoding.GetEncoding("iso-8859-1");
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            //richTextBox1.Text = responseFromServer;

            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
            //MessageBox.Show(responseFromServer);
            data = responseFromServer.Split(',');
            //List<string> gelenData = new List<string>(data); 
            return data;
        }
    }
}
