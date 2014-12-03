// <copyright file="CommentsSurfaceController.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using umbraco.presentation.nodeFactory;
    using Umbraco.Core.Models;

    /// <summary>
    /// Model representing the related news item list
    /// </summary>
    public class RelatedNewsItemViewModel
    {
        public IEnumerable<IPublishedContent> relatedNewsItemList;

        
    }
}