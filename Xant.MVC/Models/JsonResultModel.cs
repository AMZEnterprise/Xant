namespace Xant.MVC.Models
{
    public class JsonResultModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
    }
}
