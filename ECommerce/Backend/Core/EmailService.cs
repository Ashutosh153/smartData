using App.core.interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App.core
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }
        public async Task<bool> SendEmailAsync(string subject, string to, string body)
        {
            if (string.IsNullOrEmpty(to))
            {
                throw new ArgumentNullException("receiver is not mentioned!!!");

            }

            var smtphost = _configuration["Smtp:Host"];
            var smtpPort = _configuration["Smtp:Port"];

            var smtpEnableSsl = _configuration["Smtp:EnableSsl"];

            var smtpEmail = _configuration["Smtp:Email"];

            var smtpPassword = _configuration["Smtp:Password"];

            var smtpFrom = _configuration["Smtp:From"];

            if (string.IsNullOrEmpty(smtphost) || string.IsNullOrEmpty(smtpPort)
                || string.IsNullOrEmpty(smtpEnableSsl) || string.IsNullOrEmpty(smtpEmail)
                || string.IsNullOrEmpty(smtpPassword) || (string.IsNullOrEmpty(smtpFrom)))
            {
                _logger.LogError(" some information is missing or invalid in email service");

                throw new ArgumentNullException(" some information is missing or invalid ");

            }

            var smtpClient = new SmtpClient(smtphost)
            {
                Port = int.Parse(smtpPort),
                Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                EnableSsl = bool.Parse(smtpEnableSsl)

            };

            var MailMessage = new MailMessage
            {
                From = new MailAddress(smtpFrom),
                Body = body,
                Subject = subject,
                IsBodyHtml = true
                

            };
            MailMessage.To.Add(to);
            try
            {
                await smtpClient.SendMailAsync(MailMessage);
                _logger.LogInformation("email send successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("unexpected error occured while sending the email",ex.Message);
                return false;
               // throw new InvalidOperationException($"unexpected error occured while sending the email {to}:{ex.Message}",ex);
                
            }


        }
    }
}
