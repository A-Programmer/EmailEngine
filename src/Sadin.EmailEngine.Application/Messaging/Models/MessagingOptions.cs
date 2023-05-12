namespace Sadin.EmailEngine.Application.Messaging.Models;

public record MessagingOptions
{
    // Check whether this works or not without having this constant property.
    // public const string Messaging = "Messaging";
    public string HostName { get; set; }
    public int HostPort { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}