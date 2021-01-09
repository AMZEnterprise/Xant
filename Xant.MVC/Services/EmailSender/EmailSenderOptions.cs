namespace Xant.MVC.Services.EmailSender
{
    /// <summary>
    /// Default options used by EmailSender.
    /// </summary>
    public class EmailSenderOptions
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
    }
}
