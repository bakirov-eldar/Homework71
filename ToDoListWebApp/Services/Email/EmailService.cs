using Microsoft.Extensions.Options;

namespace ToDoListWebApp.Services.Email;

public class EmailService
{
    private EmailOptions options;
    public EmailService(IOptions<EmailOptions> options)
    {
        this.options = options.Value;
    }
    public void SendEmail(string email, string subject, string text)
    {

        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        string to = email;

        var message = new MimeKit.MimeMessage();
        message.From.Add(new MimeKit.MailboxAddress(options.FromName, options.EmailAddress));
        message.To.Add(new MimeKit.MailboxAddress("", to));
        message.Subject = subject;
        message.Body = new MimeKit.TextPart("html")
        {
            Text = text
        };

        using (var client = new MailKit.Net.Smtp.SmtpClient())
        {
            client.Connect(options.ServerAddress, options.ServerPort, options.EnableSsl);
            client.Authenticate(options.EmailAddress, options.EmailPassword);
            client.Send(message);
            client.Disconnect(true);
        }

    }
}
