using System;
using System.Data;
using System.Timers;
using System.Net;
using System.Net.Mail;

namespace STROC_SendMail
{

    public class Example
    {

        private static System.Timers.Timer aTimer;

        public static void Main()
        {
            csOperaciones ope = new csOperaciones();

            SetTimer();
            //Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("Inicio de aplicación - Hora ** {0:HH:mm:ss.fff} **", DateTime.Now);
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();
            Console.WriteLine("Terminating the application...");
        }

        private static void SetTimer()
        {
            //Convertir de minutos a milisegundos

            double intervalo = TimeSpan.FromMinutes(30).TotalMilliseconds;
            aTimer = new System.Timers.Timer(intervalo);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine("Consulta {0:HH:mm:ss.fff}", e.SignalTime);
            csOperaciones ope = new csOperaciones();
            
            string sql01 = "SELECT DISTINCT Estatus FROM tmpFlujo";
            DataTable Result01 =  ope.ArmaConsulta(sql01);
            string Estatus;
            for(int i = 0; i < Result01.Rows.Count; i++)
            {
                Estatus = Result01.Rows[i][0].ToString();
                if (Estatus == "1")
                {
                    Console.WriteLine("**************************************************");
                    string sql02 = "SELECT DISTINCT Aprobador FROM tmpFlujo WHERE Estatus = 1";
                    DataTable Result02 = ope.ArmaConsulta(sql02);
                    string aprob;
                    for( int j = 0; j < Result02.Rows.Count; j++)
                    {
                        aprob = Result02.Rows[j][0].ToString();                          
                        string sql03 = "SELECT Mail FROM tbpersonas WHERE Nomina = '"+aprob+"'";
                        DataTable Result03 = ope.ArmaConsulta(sql03);
                        string mail = Result03.Rows[j]["Mail"].ToString();
                        //Console.WriteLine(mail);                                                               
                        sendMail(mail);                   
                    }
                }
                else if (Estatus == "10")
                {
                    Console.WriteLine("**************************************************");
                    string sql02 = "SELECT DISTINCT Aprobador FROM tmpFlujo WHERE Estatus = 10";
                    DataTable Result02 = ope.ArmaConsulta(sql02);
                    string aprob;
                    for (int j = 0; j < Result02.Rows.Count; j++)
                    {
                        aprob = Result02.Rows[j][0].ToString();
                        string sql03 = "SELECT Mail FROM tbpersonas WHERE Nomina = '" + aprob + "'";
                        DataTable Result03 = ope.ArmaConsulta(sql03);
                        string mail = Result03.Rows[j]["Mail"].ToString();
                        //Console.WriteLine(mail);                                                               
                        sendMail(mail);
                    }
                }
                else
                {
                    Console.WriteLine("**************************************************");
                    string sql04 = "SELECT DISTINCT tbpersonas.Mail FROM tbpersonas " +
                    "JOIN tbusuarios ON tbpersonas.Nomina = tbusuarios.usuario " +
                    "JOIN tbEstatus ON tbusuarios.nivel = tbEstatus.nivel " +
                    "JOIN tmpFlujo ON tbEstatus.EstatusID = tmpFlujo.Estatus " +
                    "WHERE tmpFlujo.Estatus = " + Convert.ToInt16(Estatus);
                    DataTable Result04 = ope.ArmaConsulta(sql04);
                    for (int j = 0; j < Result04.Rows.Count; j++)
                    {
                        string mail = Result04.Rows[j]["Mail"].ToString();
                        //Console.WriteLine(mail);
                        sendMail(mail);                                                                
                    }
                }                
            }
            Int16 Resultado = ope.deleteTmpTable();
            if(Resultado != 1)
            {
                Console.WriteLine("No se pueden borrar los datos");
            }
            Console.WriteLine("tmpFlujo eliminada");
        }
        static void sendMail(String _mail)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("shopfloor_service@konfe.dom");
            mail.To.Add(_mail);
            mail.Subject = "STROC";
            mail.Body = "<h3><b>Tienes requisiciones por aprobar</b></h3> <h5><p><i>Favor de no responder este correo.</i></p></h5> <h5><p><i>Atte. IT-Shopfloor</i></p></h5>";
            mail.IsBodyHtml = true;
            /*
            string body = "Tienes requisiciones por aprobar.  \n\n Favor de no responder este correo. \n Atte. IT-Shopfloor.";
            string subjet = "STROC";
            string from = "shopfloor_service@konfe.dom";
            string to = _mail;
            */
            try
            {
                var smtpClient = new SmtpClient("192.1.1.30")
                {
                    Port = 25,
                    EnableSsl = false,
                };
                smtpClient.Send(mail);
                Console.WriteLine("Correo enviado a **" +_mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

    }
}
