using System;
using System.Net;
using System.Net.Mail;
using API.Interfaces;

namespace API.Services;

public class EmailService: IEmailService
{
    private readonly IConfiguration configuration;

    public EmailService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string body,
        bool isHtml = false)
    {
        var smtpHost = configuration["Email:SmtpHost"];
        var smtpPort = int.Parse(configuration["Email:SmtpPort"]!);
        var username = configuration["Email:Username"];
        var password = configuration["Email:Password"];

        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = true
        };

         using var message = new MailMessage
        {
            From = new MailAddress(username!),
            Subject = subject,
            Body = body,
            IsBodyHtml = isHtml
        };

        message.To.Add(to);

        await client.SendMailAsync(message);
    }
}