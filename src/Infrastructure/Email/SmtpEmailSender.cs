using System.Net;
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
        var fromAddress = GetRequiredSetting("Smtp:From");
        var toAddress = GetRequiredSetting("Smtp:To");
        var host = GetRequiredSetting("Smtp:Host");
        var user = GetRequiredSetting("Smtp:User");
        var password = GetRequiredSetting("Smtp:Password");

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Iliya Ahmadi · Portfolio", fromAddress));
        email.To.Add(MailboxAddress.Parse(toAddress));
        email.ReplyTo.Add(new MailboxAddress(message.Name, message.Email));
        email.Subject = $"New portfolio inquiry from {message.Name}";

        var safeName = WebUtility.HtmlEncode(message.Name);
        var safeEmail = WebUtility.HtmlEncode(message.Email);
        var safeEmailUri = Uri.EscapeDataString(message.Email);
        var safeMessage = WebUtility.HtmlEncode(message.Message)
            .Replace("\r\n", "<br>")
            .Replace("\n", "<br>");

        var body = new BodyBuilder
        {
            TextBody = $"New message from your portfolio\n\nName: {message.Name}\nEmail: {message.Email}\n\nMessage:\n{message.Message}\n\nReply to this email to respond directly to {message.Name}.",
            HtmlBody = $$"""
                <!doctype html>
                <html lang="en">
                <body style="margin:0;background:#f1f3ee;color:#1c211d;font-family:Arial,sans-serif;">
                    <div style="max-width:620px;margin:0 auto;padding:40px 20px;">
                        <div style="overflow:hidden;border:1px solid #dce2da;border-radius:16px;background:#ffffff;">
                            <div style="padding:26px 30px;background:#1c211d;color:#f4f6f2;">
                                <div style="font-size:12px;color:#a9b8ac;letter-spacing:.08em;text-transform:uppercase;">Portfolio contact</div>
                                <h1 style="margin:10px 0 0;font-size:23px;line-height:1.35;">New inquiry from {{safeName}}</h1>
                            </div>
                            <div style="padding:28px 30px;">
                                <table role="presentation" style="width:100%;margin-bottom:24px;border-collapse:collapse;font-size:14px;">
                                    <tr><td style="width:70px;padding:5px 0;color:#738076;">Name</td><td style="padding:5px 0;font-weight:600;">{{safeName}}</td></tr>
                                    <tr><td style="padding:5px 0;color:#738076;">Email</td><td style="padding:5px 0;"><a href="mailto:{{safeEmailUri}}" style="color:#315f3e;">{{safeEmail}}</a></td></tr>
                                </table>
                                <div style="padding:20px;border-radius:10px;background:#f4f6f2;font-size:15px;line-height:1.7;">{{safeMessage}}</div>
                                <a href="mailto:{{safeEmailUri}}" style="display:inline-block;margin-top:24px;padding:12px 18px;border-radius:8px;background:#315f3e;color:#ffffff;text-decoration:none;font-size:14px;font-weight:600;">Reply to {{safeName}}</a>
                            </div>
                        </div>
                        <p style="margin:16px 0 0;text-align:center;color:#7e8980;font-size:12px;">Sent through iliyaahmadi.com</p>
                    </div>
                </body>
                </html>
                """
        };
        email.Body = body.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(host, int.Parse(GetRequiredSetting("Smtp:Port")), MailKit.Security.SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(user, password);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }

    private string GetRequiredSetting(string key) =>
        _config[key] ?? throw new InvalidOperationException($"Missing required configuration setting '{key}'.");
}
