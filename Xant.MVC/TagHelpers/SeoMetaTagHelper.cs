using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Xant.MVC.TagHelpers
{
    /// <summary>
    /// Seo tag helper , use to inject meta tags
    /// </summary>
    [HtmlTargetElement("Seo-Meta", Attributes = "Description")]
    public class SeoMetaTagHelper : TagHelper
    {
        public string Description { get; set; }
        public string KeyWords { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public string Type { get; set; }
        public string OgDescription { get; set; }
        public string Local { get; set; }
        public string CanonicalLink { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string html = string.Empty;

            if (!string.IsNullOrEmpty(Description))
            {
                html += $"<meta name=\"description\" content =\"{Description}\" />";
            }

            if (!string.IsNullOrEmpty(KeyWords))
            {
                html += $"<meta name=\"keywords\" content =\"{KeyWords}\" />";
            }

            if (!string.IsNullOrEmpty(Title))
            {
                html += $"\n<meta property=\"og:title\" content=\"{Title}\" />";
            }

            if (!string.IsNullOrEmpty(Url))
            {
                html += $"\n<meta property=\"og:url\" content=\"{Url}\" />";
            }


            if (!string.IsNullOrEmpty(Image))
            {
                html += $"\n<meta property=\"og:image\" content=\"{Image}\" />";
            }

            if (!string.IsNullOrEmpty(Type))
            {
                html += $"\n<meta property=\"og:type\" content=\"{Type}\" />";
            }

            if (!string.IsNullOrEmpty(OgDescription))
            {
                html += $"\n<meta property=\"og:description\" content=\"{OgDescription}\" />";
            }

            if (!string.IsNullOrEmpty(Local))
            {
                html += $"\n<meta property=\"og:local\" content=\"{Local}\" />";
            }

            if (!string.IsNullOrEmpty(CanonicalLink))
            {
                html += $"\n<link rel=\"canonical\" href=\"{CanonicalLink}\" />";
            }

            output.TagName = null;
            output.Content.AppendHtml(html);
        }
    }
}
