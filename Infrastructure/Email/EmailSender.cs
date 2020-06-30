﻿using System;
using System.IO;
using System.Security.Authentication;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace Infrastructure.Email
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _configuration;

        public EmailSender(EmailConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<EmailResponseModel> SendAsync(string toAddress, string subject, string body, string attachmentPath = null)
        {
            using var client = new SmtpClient();
            Enum.TryParse(_configuration.SslProtocol, out SslProtocols sslProtocols);
            client.SslProtocols = sslProtocols;
            await client.ConnectAsync(_configuration.Host, _configuration.Port, _configuration.EnableSsl);
            await client.AuthenticateAsync(_configuration.UserName, _configuration.Password);
            await client.SendAsync(MimeMessage(toAddress, subject, body, attachmentPath));
            await client.DisconnectAsync(true);
            
            return new EmailResponseModel();
        }

        public EmailResponseModel Send(string toAddress, string subject, string body, string attachmentPath = null)
        {
            using var client = new SmtpClient();
            Enum.TryParse(_configuration.SslProtocol, out SslProtocols sslProtocols);
            client.SslProtocols = sslProtocols;
            client.Connect(_configuration.Host, _configuration.Port, _configuration.EnableSsl);
            client.Authenticate(_configuration.UserName, _configuration.Password);
            client.Send(MimeMessage(toAddress, subject, body, attachmentPath));
            client.Disconnect(true);
            
            return new EmailResponseModel();
        }

        private MimeMessage MimeMessage(string toAddress, string subject, string body,  string attachmentPath = null)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_configuration.EmailFrom));
            mimeMessage.To.Add(new MailboxAddress(toAddress));
            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body
            };

            if (attachmentPath != null)
            {
                bodyBuilder.Attachments.Add(BuildFile(attachmentPath));
            }

            mimeMessage.Body = bodyBuilder.ToMessageBody();

            return mimeMessage;
        }

        private MimePart BuildFile(string attachmentPath)
        {
            var attachmentFileName = Path.GetFileName(attachmentPath);
            using var attachmentStream = File.OpenRead(attachmentPath);
            var attachment = new MimePart
            {
                Content = new MimeContent(attachmentStream),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = attachmentFileName
            };

            return attachment;
        }
    }
}