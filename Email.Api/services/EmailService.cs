

using MailKit.Net.Smtp;
using MimeKit;

namespace Email.Api.services;

public class EmailService
{
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _smtpUser;
    private readonly string _smtpPass;

    public EmailService(string? smtpServer, int smtpPort, string? smtpUser, string smtpPass)
    {
        _smtpServer = smtpServer!;
        _smtpPort = smtpPort;
        _smtpUser = smtpUser!;
        _smtpPass = smtpPass;

    }
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Email Service", _smtpUser));
        message.To.Add(new MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using (var client = new SmtpClient())
        {
            await client.ConnectAsync(_smtpServer, _smtpPort, false);
            await client.AuthenticateAsync(_smtpUser, _smtpPass);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
