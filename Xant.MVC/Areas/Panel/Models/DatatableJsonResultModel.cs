namespace Xant.MVC.Areas.Panel.Models
{
    /// <summary>
    /// Json result object model for jQuery datatable
    /// </summary>
    public class DatatableJsonResultModel
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public object Data { get; set; }
    }
}
