using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IS_KASSE
{
    class Log
    {
        public void AddtoLogFile(string Message, string Ort)
        {
            string LogPath = Application.StartupPath.ToString();
            string filename = "Log_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
            string filepath = LogPath + "\\LOG\\" + filename;
            if (!Directory.Exists(LogPath + "\\LOG\\"))
            {
                Directory.CreateDirectory(LogPath + "\\LOG\\");
            }
            if (File.Exists(filepath))
            {
                using (StreamWriter writer = new StreamWriter(filepath, true))
                {
                    writer.WriteLine("[START:" + DateTime.Now);
                    writer.WriteLine("Source :" + Ort + "\n");
                    writer.WriteLine(Message);
                    writer.WriteLine("END]");
                }
            }
            else
            {
                StreamWriter writer = File.CreateText(filepath);
                writer.WriteLine("[START:" + DateTime.Now);
                writer.WriteLine("Source :" + Ort + "\n");
                writer.WriteLine(Message);
                writer.WriteLine("END]");
                writer.Close();
            }
        }
    }
}
