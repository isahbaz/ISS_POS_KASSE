using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace IS_KASSE
{
    public static class AnadoluMail
    {
        public static bool mopinGuthabenMail(string html)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.iss-pos.de");

                mail.From = new MailAddress("info@iss-pos.de");
                mail.To.Add("tnblt1976@gmail.com");
                mail.To.Add("ihsanisahbaz@gmail.com");
                mail.Subject = "ISS-POS KASSENSYSTEM Täglich INFO";
                //mail. = "ISS KASSENSYSTEM";
                mail.IsBodyHtml = true;
                string htmlBody;

                htmlBody = html;

                mail.Body = htmlBody;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("info@iss-pos.de", "Ab3420351?");
                //SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public static bool TSE_ErrorMail(string html)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.iss-pos.de");

                mail.From = new MailAddress("tse-error@iss-pos.de");
                mail.To.Add("tse-error@iss-pos.de");
                mail.To.Add("info@iss-pos.de");
                mail.To.Add(Program.IsletmeAyarlar["mail"]);
                mail.Subject = Program.IsletmeAyarlar["isletme"]+"-ISS-POS KASSENSYSTEM TSE Error Nachricht";
                //mail. = "ISS KASSENSYSTEM";
                mail.IsBodyHtml = true;
                string htmlBody;

                htmlBody = html;

                mail.Body = htmlBody;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("tse-error@iss-pos.de", "MusteriBilgi123?");
                //SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public static bool ISSAPI_ErrorMail(string html)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.iss-pos.de");

                mail.From = new MailAddress("tse-error@iss-pos.de");
                //mail.To.Add("tse-error@iss-pos.de");
                mail.To.Add("info@iss-pos.de");
                //mail.To.Add(Program.IsletmeAyarlar["mail"]);
                mail.Subject = Program.IsletmeAyarlar["isletme"] + "-ISS-POS KASSENSYSTEM ISS API Error Nachricht";
                //mail. = "ISS KASSENSYSTEM";
                mail.IsBodyHtml = true;
                string htmlBody;

                htmlBody = html;

                mail.Body = htmlBody;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("tse-error@iss-pos.de", "MusteriBilgi123?");
                //SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public static bool MopinGuthabenEmail(string html)
        {
            try
            {
              /*  MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.iss-pos.de");

                mail.From = new MailAddress("info@iss-pos.de");
                mail.To.Add("info@mopin.de");
                mail.To.Add("info@iss-pos.de");
                mail.To.Add(Program.IsletmeAyarlar["mail"]);
                mail.Subject = "GUTHABEN ALERT - "+Program.IsletmeAyarlar["isletme"] + "";
                //mail. = "ISS KASSENSYSTEM";
                mail.IsBodyHtml = true;
                string htmlBody;

                htmlBody = html;

                mail.Body = htmlBody;

                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("info@iss-pos.de", "Ab3420351?");
                //SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);*/
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
