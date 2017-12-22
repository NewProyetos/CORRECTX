using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;


namespace Sinconizacion_EXactus
{
    
    class SMTP
      
    {
        public SmtpClient smtp1 = new SmtpClient();

        public MailMessage email = new MailMessage();
        Conexion2 conet = new Conexion2();


        string mail_detalle = "";
        //string mail_Equipo;
        //string mail_Nombre;
        //string mail_Ruta;

        public SMTP()
        {

            



            
            email.To.Add(new MailAddress("isaac_turcios@lamorazan.com"));
            // email.CC.Add(new MailAddress("example@example.com"));
            //email.CC.Add(new MailAddress("example@example.com"));
            email.From = new MailAddress("isaac_turcios@lamorazan.com");
            email.Subject = "USUARIO ("+Login.usuario.ToUpper()+")  ha ingresado un nuevo caso a la Ruta ";
            email.Body = "Se ha agregado un Nuevo Caso con el siguente Contenido  Usuaio:"+Casos_Main.mail_Nombre+"  Equipo: "+" "+Casos_Main.mail_equipo+"  DETALLE: "+mail_detalle+"";

            email.IsBodyHtml = true;
            email.Priority = MailPriority.Normal;

                        
         smtp1.Host = "smtpout.secureserver.net";
         smtp1.Port = 80;
         smtp1.EnableSsl =false;
         smtp1.UseDefaultCredentials = false;

         smtp1.Credentials = new NetworkCredential("isaac_turcios@lamorazan.com", "Newtron30");


        }


              
            
        
    }
}
