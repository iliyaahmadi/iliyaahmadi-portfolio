using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Contact;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Email;

public sealed class ResendEmailSender : IEmailSender
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ResendEmailSender(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendAsync(ContactMessage message)
    {
        var apiKey = GetRequiredSetting("Resend:ApiKey");
        var fromAddress = GetRequiredSetting("Resend:From");
        var toAddress = GetRequiredSetting("Resend:To");

        var safeName = WebUtility.HtmlEncode(message.Name);
        var safeEmail = WebUtility.HtmlEncode(message.Email);
        var safeEmailUri = Uri.EscapeDataString(message.Email);
        var safeMessage = WebUtility.HtmlEncode(message.Message)
            .Replace("\r\n", "<br>")
            .Replace("\n", "<br>");

        var payload = new
        {
            from = fromAddress,
            to = new[] { toAddress },
            reply_to = message.Email,
            subject = $"New portfolio inquiry from {message.Name}",
            text = $"New message from your portfolio\n\nName: {message.Name}\nEmail: {message.Email}\n\nMessage:\n{message.Message}\n\nReply to this email to respond directly to {message.Name}.",
            html = $$"""
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
                        <p style="margin:16px 0 0;text-align:center;color:#7e8980;font-size:12px;">Sent through Iliya Ahmadi's portfolio</p>
                    </div>
                </body>
                </html>
                """
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "emails")
        {
            Content = JsonContent.Create(payload)
        };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        using var response = await _httpClient.SendAsync(request);
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        throw new InvalidOperationException(
            $"Resend rejected the contact email with status {(int)response.StatusCode}: {responseBody}");
    }

    private string GetRequiredSetting(string key) =>
        !string.IsNullOrWhiteSpace(_configuration[key])
            ? _configuration[key]!
            : throw new InvalidOperationException($"Missing required configuration setting '{key}'.");
}
