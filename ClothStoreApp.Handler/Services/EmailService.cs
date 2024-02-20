using ClothStoreApp.Share.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ClothStoreApp.Handler.Services
{
    public class Message
    {
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(t => new MailboxAddress(t)));

            Subject = subject;
            Content = content;
        }
    }

    public interface IEmailService
    {
        Task<bool> SendEmailAsync(Message message);
        bool SendEmail(Message message);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        
        public EmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.FromUser));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        public bool SendEmail(Message message)
        {
            var smtpMessage = CreateEmailMessage(message);
            try
            {
                Send(smtpMessage);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public async Task<bool> SendEmailAsync(Message message)
        {
            var smtpMessage = CreateEmailMessage(message);
            try
            {
                await SendAsync(smtpMessage);
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void Send(MimeMessage message)
        {
            using(var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(message);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
        private async Task SendAsync(MimeMessage message)
        {
            using(var smtpClient = new SmtpClient())
            {
                try
                {
                    await smtpClient.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    smtpClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    await smtpClient.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);

                    await smtpClient.SendAsync(message);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    await smtpClient.DisconnectAsync(true);
                    smtpClient.Dispose();
                }
            }
            
        }
    }

}
