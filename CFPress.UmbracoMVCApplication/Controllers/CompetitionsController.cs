// <copyright file="CommentsController.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml.Linq;
    using Umbraco.Web;
    using Umbraco.Web.Models;
    using Umbraco.Web.Mvc;
    using CFPress.UmbracoMVCApplication.Models;
    using CFPress.UmbracoMVCApplication.Services;
    using CFPress.UmbracoMVCApplication.Utilities;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;
    using umbraco;
    using Umbraco.Core.IO;

    public class CompetitionsController : BaseController
    {
        private readonly CompetitionsService competitionsService;
        BaseController basecontroller = new BaseController();
        FormDetailsViewModel formdetailsmodel = new FormDetailsViewModel();
        
        /// <summary>
        /// content type service
        /// </summary>
        private static ContentTypeService contentTypeService = new ContentTypeService(contentService, mediaService);
      
        public CompetitionsController(UmbracoContext umbracoContext)
            : base(umbracoContext)
        {

        }

        public CompetitionsController(ICompetitionsService icompetitionsService)
        {
            competitionsService = (CompetitionsService)icompetitionsService;
            competitionsService.umbDatabase = ApplicationContext.DatabaseContext.Database;
        }

        public CompetitionsController()
        {
            
        }

        void ContentService_Saved(IContentService sender, Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            //// create relation between the competitions content and the member
            if(e.SavedEntities.FirstOrDefault().ContentType.Alias=="umbGalleryItem")
            {
                IRelationType relationType = relationService.GetRelationTypeByAlias("CompetitionsMemberRelation");
                IRelation relation;
                if (relationType == null)
                {
                    relationType = new RelationType(uQuery.UmbracoObjectType.Document.GetGuid(),uQuery.UmbracoObjectType.Member.GetGuid(), "CompetitionsMemberRelation");
                    relationService.Save(relationType);
                    relation = new Relation(e.SavedEntities.FirstOrDefault().Id, Members.GetCurrentMember().Id, relationType);
                    relationService.Save(relation);
                }

                else
                {
                    //// hardcoding memberid for now
                    relation = new Relation(e.SavedEntities.FirstOrDefault().Id, 1218, relationType);
                    relationService.Save(relation);
                }
            }
        }
      

        /// <summary>
        /// Renders comments posted by the logged in member
        /// </summary>
        /// <param name="model">The model with values filled in the form</param>
        /// <returns>Returns the relevant page or partial view</returns>
        public ActionResult RenderCompetitions(CompetitionsViewModel model)
        {
            //model.UmbracoNewsItemId = CurrentPage.Id;
            //var comments = this.commentsService.GetCommentsByNewsItemId(model.UmbracoNewsItemId);
            //model.CommentItems = comments;
            return this.PartialView("Competitions", model);
        }

        /// <summary>
        ///  Handles the saving the posted comment in the table
        /// </summary>
        /// <param name="model">The model with values filled in the form</param>
        /// <returns>Returns the relevant page or partial view</returns>
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Submit")]
        public ActionResult HandleDetailsPost(CompetitionsViewModel model)
        {
            ContentService.Saved += ContentService_Saved;
            //// Check if the data on the model is valid
            if (!ModelState.IsValid)
            {
                return this.PartialView("Competitions", model);
            }
            else
            {
                try
                {
                    //// set the model properties
                    //// handle null values
                    model.CompetitionName = !string.IsNullOrEmpty(CurrentPage.Name) ? CurrentPage.Name : string.Empty;
                    model.NewspaperName = !string.IsNullOrEmpty(CurrentPage.Parent.Name) ? CurrentPage.Parent.Name : string.Empty;

                    //// Check if there was file upload. If yes, then save the path
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];

                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                            file.SaveAs(path);
                            System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream);
                            model.FormDetails.ImageHeight = image.Height;
                            model.FormDetails.ImageWidth = image.Width;
                            model.FormDetails.ImageSize = file.InputStream.Length;
                            model.FormDetails.ImageType = file.ContentType;
                            model.FormDetails.ImagePath = path;

                        }

                        //// set the properties for form details
                        model.FormDetails.ImagePath = !string.IsNullOrEmpty(model.FormDetails.ImagePath) ? model.FormDetails.ImagePath : string.Empty;
                        model.Formdetailsxml = model.SerializeXml(model).ToString();

                        //// Add the entry to the db
                        int entryId = this.competitionsService.AddNewCompetitionEntry(model);
                        //// display details successfully saved message
                        //// once entry saved to the db, add the image in the incoming media folder in back end
                        if (model.FormDetails.ImagePath != null)
                        {
                            // 1108 - is hard coded here. It is the id of the incoming media folder created in umbraco back end to store incoming media
                            // media folder is the same for all the websites/newspaper
                            // first add the image to the media library

                            basecontroller.media = mediaService.CreateMedia(model.FormDetails.Name + "_" + Convert.ToString(entryId), 1108, "Image", 0);
                            basecontroller.media.SetValue("umbracoFile", model.FormDetails.ImagePath);
                            basecontroller.media.SetValue("umbracoWidth", model.FormDetails.ImageWidth);
                            basecontroller.media.SetValue("umbracoHeight", model.FormDetails.ImageHeight);
                            basecontroller.media.SetValue("umbracoBytes", model.FormDetails.ImageSize);
                            mediaService.Save(basecontroller.media, 0, false);


                            //get the parent Id dynamically for each website - as in each newspaper
                            var parentcontent = from pcontent in contentService.GetByLevel(2) where pcontent.ContentType.Alias == "umbGalleryOverview" select pcontent;

                            // get the home page or root content id for each newspaper by the website name as read from the xml
                            var masterContentId = contentService.GetRootContent().FirstOrDefault<IContent>(m => m.Name == model.NewspaperName).Id;

                            // get the parent node id - the News Overview item item below which each  news item has to be published
                            var parentId = parentcontent.FirstOrDefault<IContent>(m => m.ParentId == masterContentId).Id;
                            basecontroller.content = contentService.CreateContent("model.CompetitionName" + "_" + Convert.ToString(entryId), parentId, contentTypeService.GetContentType("umbGalleryItem").Alias, 0);

                            basecontroller.content.SetValue("image", file);
                            //populate the content with the class attributes
                            basecontroller.content.CreateDate = System.DateTime.Now;
                            basecontroller.content.Name = model.CompetitionName + "_" + Convert.ToString(entryId);
                            basecontroller.content.WriterId = 0;
                            contentService.Save(basecontroller.content, 0, true);

                        }
                    }

                    else
                    {
                        ModelState.AddModelError("Upload Image", "Please upload an image");
                    }
                }

                catch (Exception ex)
                {
                    ModelState.AddModelError("Failed to save the details", ex.Message);
                }

                //// All done - redirect to render the entry
                return this.RedirectToCurrentUmbracoPage();
            }
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "File")]
        public ActionResult Upload()
        {
            return RedirectToAction("UploadDocument");
        }


        /// <summary>
        ///  Handles the saving the posted comment in the table
        /// </summary>
        /// <param name="model">The model with values filled in the form</param>
        /// <returns>Returns the relevant page or partial view</returns>
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Vote")]
        public ActionResult HandleVoting(CompetitionsViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return this.PartialView("GalleryItem");
            }

            else
            {
                try
                {
                    int entryId = int.Parse(CurrentPage.Name.Substring(CurrentPage.Name.IndexOf("_") + 1));
                    int votes = competitionsService.GetEntryById(entryId).EarnedVotes;
                    //// for every hit of the vote button, add one vote
                    votes = votes + 1;
                    competitionsService.SaveEarnedVotesByEntryId(entryId, votes);
                    //// then store the memberid in the Competitions2votingmembers table
                    competitionsService.SaveVotingMemberIdes(entryId,1285);

                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Failed saving the vote for this entry", ex.Message);
                }

            }

            return this.RedirectToCurrentUmbracoPage();
        }

     }
}

