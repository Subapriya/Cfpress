// <copyright file="CommentsController.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using umbraco;
    using Umbraco.Core;
    using Umbraco.Core.Services;
    using Umbraco.Web.Mvc;
    using CFPress.UmbracoMVCApplication.Utilities;
    using CFPress.UmbracoMVCApplication.Models;
    using CFPress.UmbracoMVCApplication.Pocos;
    using Umbraco.Web;
    using CFPress.UmbracoMVCApplication.Services;

    /// <summary>
    ///  Comments Surface controller class. Handles the comments section on the front end
    /// </summary>
    public class CommentsController : BaseController 
    {
        public IDictionary<string, object> TempDatadic;
        private readonly CommentsService commentsService;
        public CommentsController(UmbracoContext umbracoContext) : base(umbracoContext)
        {

        }
        public CommentsController(ICommentsService icommentsService)
        {
            commentsService = (CommentsService)icommentsService;
            commentsService.umbDatabase = ApplicationContext.DatabaseContext.Database;
        }

        public CommentsController()
        {
            TempDatadic = TempData;
        }
        /// <summary>
        /// Renders comments posted by the logged in member
        /// </summary>
        /// <param name="model">The model with values filled in the form</param>
        /// <returns>Returns the relevant page or partial view</returns>
        public ActionResult RenderComments(CommentsViewModel model)
        {
             model.UmbracoNewsItemId = CurrentPage.Id;
             var comments = this.commentsService.GetCommentsByNewsItemId(model.UmbracoNewsItemId);
             model.CommentItems = comments;
             return this.PartialView("Comments", model);
        }

       /// <summary>
       ///  Handles the saving the posted comment in the table
       /// </summary>
       /// <param name="model">The model with values filled in the form</param>
       /// <returns>Returns the relevant page or partial view</returns>
       [HttpPost]
       public ActionResult HandleCommentPost(CommentsViewModel model)
       {
           //// Check if the data on the model is valid
           if (!ModelState.IsValid)
           {
               return this.PartialView("Comments", model);
           }
           else
           {
               //// Members.IsMemberAuthorized() &&
               if ( Members.IsLoggedIn())
               {
                   
                   model.UmbracoNewsItemId = CurrentPage.Id;
                   model.MemberId = Members.GetCurrentMemberId();
                   model.CommentStatus = Enumerations.CommentStatus.PreApprove;

                   try
                   {
                       if(commentsService.AddNewCommentsByNewsItemIdAndMemberId(model))
                       {
                           var commentstoDisplay = commentsService.GetCommentsByNewsItemId(model.UmbracoNewsItemId);
                           model.CommentItems = commentstoDisplay;
                       }
                   }
                   catch(Exception ex)
                   {
                       
                       ModelState.AddModelError("Saving Comments To Database failed", ex.Message);
                   }
                   
                   //// All done - redirect to render comments
                   return this.RedirectToCurrentUmbracoPage();
               }
               else
               {
                   return this.CurrentUmbracoPage();
               }
           }
       }
    }
}
