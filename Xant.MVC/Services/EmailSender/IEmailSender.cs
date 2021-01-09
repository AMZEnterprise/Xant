namespace Xant.MVC.Services.EmailSender
{
    /// <summary>
    /// Use to send email for account confirmation , password reset , and other
    /// information data.
    /// </summary>
    public interface IEmailSender
    {
        bool SendEmail(string emailAddress, string subject, string message);
    }
}