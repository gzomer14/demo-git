using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoGit.Domain.Entities.DTO;
using DemoGit.Domain.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace DemoGit.Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async void SendEmail(string to, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_smtpSettings.User));

            foreach (var address in to.Split("|"))
                email.To.Add(MailboxAddress.Parse(address));

            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, int.Parse(_smtpSettings.Port ?? "587"), SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_smtpSettings.User, _smtpSettings.Password);
            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}