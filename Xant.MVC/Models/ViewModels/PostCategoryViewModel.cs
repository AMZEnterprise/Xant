using Xant.Core.Domain;

namespace Xant.MVC.Models.ViewModels
{
    public class PostCategoryViewModel : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PostsCount { get; set; }
    }
}
