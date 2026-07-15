using Application.Contact;
using Domain.Entities;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _config;

    public SmtpEmailSender(IConfiguration config) => _config = config;

    public async Task SendAsync(ContactMessage message)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_config["Smtp:From"]));
        email.To.Add(MailboxAddress.Parse(_config["Smtp:To"]));
        email.Subject = $"Portfolio contact from {message.Name}";
        email.Body = new TextPart("plain")
        {
            Text = $"From: {message.Name} <{message.Email}>\n\n{message.Message}"
        };

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_config["Smtp:Host"], int.Parse(_config["Smtp:Port"]!), MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_config["Smtp:User"], _config["Smtp:Password"]);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}