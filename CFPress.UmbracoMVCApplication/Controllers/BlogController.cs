using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace CFPress.UmbracoMVCApplication.Controllers
{
    public class Entry
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public string[] Tags { get; set; }
        public DateTime Date { get; set; }
    }

    public interface IBlogService
    {
        Entry GetBlogEntry(int id);
    }

    public class BlogEntryController : RenderMvcController
    {
        private readonly IBlogService _blogService;

        public BlogEntryController(IBlogService blogService, UmbracoContext ctx)
            : base(ctx)
        {
            _blogService = blogService;
        }

        public BlogEntryController(IBlogService blogService)
            : this(blogService, UmbracoContext.Current)
        {
        }

        public override ActionResult Index(RenderModel model)
        {
            var entry = _blogService.GetBlogEntry(model.Content.Id);

            // Test will fail if we return CurrentTemplate(model) as is expecting 
            // the action from ControllerContext.RouteData.Values["action"]
            return View("BlogEntry", entry);
        }
    }
}
