namespace Sadin.EmailEngine.Infrastructure.Models;

public sealed class CustomEmailProviderConfigurations
{
    public string From { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool EnableSsl { get; set; }
}