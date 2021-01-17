namespace Xant.MVC.Models
{
    /// <summary>
    /// Json result model
    /// </summary>
    public class JsonResultModel
    {
        public JsonResultStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public string RedirectUrl { get; set; }
    }

    /// <summary>
    /// Json result status code
    /// </summary>
    public enum JsonResultStatusCode
    {
        Success = 200,
        ModelSatedNotValid = 422,
        ServerError = 500
    }
}
