namespace Xant.MVC.Models
{
    public class PostSearch
    {
        public string SearchString { get; set; }
        public int? PostCategoryId { get; set; }
        public string PostTag { get; set; }
        public PostSortFilterType PostSortFilterType { get; set; }
    }
}
