// <copyright file="CommentsViewModel.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CFPress.UmbracoMVCApplication.Enumerations;
    using CFPress.UmbracoMVCApplication.Pocos;
    using Umbraco.Web.Models;
 
    /// <summary>
    ///  Model to represent the comments logged by registered users against any news item
    /// </summary>
    public class CommentsViewModel
    {
        /// <summary>
        ///  Gets or sets the id of the member who entered the comment
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        ///  Gets or sets the News item Id against which comment was entered
        /// </summary>
        public int UmbracoNewsItemId { get; set; }

        /// <summary>
        ///  Gets or sets The actual comment text
        /// </summary>
        [Required]
        [DisplayName("Enter your comments here")]
        public string CommentText { get; set; }

        /// <summary>
        ///  Gets or sets Collection of comments to display on the page
        /// </summary>
        public IEnumerable<Comments> CommentItems { get; set; }

        /// <summary>
        ///  Gets or sets Status of the comment
        /// </summary>
        public CommentStatus CommentStatus { get; set; }
    }
}