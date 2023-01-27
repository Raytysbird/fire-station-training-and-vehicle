using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;


namespace fire_station_training_and_vehicle.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient
            {
                Port = 587,
                Host = "smtp-mail.outlook.com", //or another email sender provider
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("jaskarankang4@outlook.com", "199strope")
            };

            return client.SendMailAsync("jaskarankang4@outlook.com", email, subject, htmlMessage);
        }
    }
}
