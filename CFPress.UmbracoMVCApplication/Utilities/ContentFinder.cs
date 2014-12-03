using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using umbraco;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace CFPress.UmbracoMVCApplication.Utilities
{
    public class ContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            var allNodes = uQuery.GetNodesByType("umbNewsItem").OrderByDescending(n => n.Level);

            foreach (var node in allNodes)
            {
                string fullUri = contentRequest.Uri.AbsolutePath;
                string parentUri = node.Url;
                bool isChild = fullUri.StartsWith(parentUri, StringComparison.InvariantCultureIgnoreCase);

                if (isChild)
                {
                    contentRequest.PublishedContent = new UmbracoHelper(UmbracoContext.Current).TypedContent(node.Id);
                    return true;
                }
            }
            return false;
        }
    }
}
