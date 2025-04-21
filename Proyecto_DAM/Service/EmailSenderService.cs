using System.Net;
using System.Net.Mail;
using System;
using System.Threading.Tasks;
using Proyecto_DAM.DTO;
using Proyecto_DAM.Interfaces;
using Proyecto_DAM.Utils;

namespace Proyecto_DAM.Services
{
    public class EmailSenderService : IEmailSenderProvider
    {
        public async Task EnviarEmail(Email email)
        {
            try
            {
                // mailtrap
                //var smtp = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
                //{
                //    Credentials = new NetworkCredential(Constantes.USER_MAIL, Constantes.USER_PASS),
                //    EnableSsl = true
                //};

                // gmail
                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(Constantes.USER_MAIL, Constantes.USER_PASS),
                    EnableSsl = true
                };

                var fromAddress = new MailAddress("GestorEstudio@Torcal.com", "GestionEstudio");
                var toAddress = new MailAddress(email.To);

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = email.Subject,
                    Body = email.Body
                };

                await smtp.SendMailAsync(message);
                Console.WriteLine($"[✓] Correo enviado a: {email.To}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[✗] Error al enviar correo: {ex.Message}");
            }
        }
    }
}
