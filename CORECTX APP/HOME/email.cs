using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
namespace Sinconizacion_EXactus
{

    class email
    {

        


        public static void Sendtabla(DataTable dt,string Destinatario,string CC,string Asunto)
        {

                    SmtpClient smtp1 = new SmtpClient();
                    MailMessage emails = new MailMessage();
            
            try
            {
                emails.From = new MailAddress("admindm@lamorazan.com");
                emails.To.Add(new MailAddress(Destinatario));
                emails.CC.Add(new MailAddress(CC));
             

                emails.IsBodyHtml = true;
                emails.Body = DToHtml(dt);
                emails.Subject = Asunto;

                smtp1.Host = "smtpout.secureserver.net";
                smtp1.Port = 25;
                smtp1.EnableSsl = false;
                smtp1.UseDefaultCredentials = false;

                smtp1.Credentials = new NetworkCredential("admindm@lamorazan.com", "Ma1lAdw1uDM");
                smtp1.Send(emails);
            }
            catch (Exception e)
            {
                MessageBox.Show("No se envio el correo",e.ToString());
            }

        }













        public static string DToHtml(DataTable dt)
        {
            StringBuilder strHTMLBuilder = new StringBuilder();
            strHTMLBuilder.Append("<html >");
            strHTMLBuilder.Append("<head>");
            strHTMLBuilder.Append("</head>");
            strHTMLBuilder.Append("<body>");
            strHTMLBuilder.Append("<table border='1px' cellpadding='1' cellspacing='1' bgcolor='lightyellow' style='font-family:Garamond; font-size:smaller'>");

            strHTMLBuilder.Append("<tr >");
            foreach (DataColumn myColumn in dt.Columns)
            {
                strHTMLBuilder.Append("<td >");
                strHTMLBuilder.Append(myColumn.ColumnName);
                strHTMLBuilder.Append("</td>");

            }
            strHTMLBuilder.Append("</tr>");


            foreach (DataRow myRow in dt.Rows)
            {

                strHTMLBuilder.Append("<tr >");
                foreach (DataColumn myColumn in dt.Columns)
                {
                    strHTMLBuilder.Append("<td >");
                    strHTMLBuilder.Append(myRow[myColumn.ColumnName].ToString());
                    strHTMLBuilder.Append("</td>");

                }
                strHTMLBuilder.Append("</tr>");
            }

            //Close tags.   
            strHTMLBuilder.Append("</table>");
            strHTMLBuilder.Append("</body>");
            strHTMLBuilder.Append("</html>");

            string Htmltext = strHTMLBuilder.ToString();

            return Htmltext;

        }  




    }
}
