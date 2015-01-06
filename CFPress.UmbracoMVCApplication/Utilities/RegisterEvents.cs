using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Persistence;
using Umbraco.Core.Persistence.DatabaseAnnotations;
using CFPress.UmbracoMVCApplication.Pocos;
using Umbraco.Core.Services;
using CFPress.UmbracoMVCApplication.Models;
using Umbraco.Web.Routing;
using umbraco.NodeFactory;
using umbraco;
using Umbraco.Core.Models;
using System.IO;
using CFPress.UmbracoMVCApplication.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CFPress.UmbracoMVCApplication.RegisterEvents
{
    public class RegisterEvents : ApplicationEventHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="umbracoApplication"></param>
        /// <param name="applicationContext"></param>
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarting(umbracoApplication, applicationContext);
                
            //// ContentFinderResolver.Current.InsertType<UmbracoMVCTest.Events.ContentFinder>();#
            ContentService.Publishing += ContentService_Publishing;
            ContentService.Saving += ContentService_Saving;
        }

        void ContentService_Saving(IContentService sender, Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            
        }

        void ContentService_Publishing(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<IContent> e)
        {
           //if sender is of type news item overview, only publish children nodes based on the number property
            if (e.PublishedEntities.FirstOrDefault().ContentType.Alias == "umbGalleryOverview")
            {
                IContent maincontent = e.PublishedEntities.FirstOrDefault();
                int selectedEntries = Convert.ToInt32(maincontent.GetValue("entriestopublish"));
                IContentService contentService = ApplicationContext.Current.Services.ContentService;
                var children =  contentService.GetDescendants(maincontent.Id).OrderByDescending(x=>x.GetValue("vote")).Take(selectedEntries);
                sender.PublishWithChildren(children, 0);
            }

            // save the media file to the azure blob storage account on content save
            var appsettings = System.Configuration.ConfigurationManager.AppSettings;
            string[] result = appsettings["StorageConnectionString"].Split(new char[] { ';' }, StringSplitOptions.None);
            BlobStorageAccessMethods blobaccessmethods = new BlobStorageAccessMethods(result[1], result[2], "images");
            blobaccessmethods.ContainerName = "images";
            blobaccessmethods.RunAtAppStartup(result[1], result[2], "images");
            string imagepath = e.PublishedEntities.FirstOrDefault().Properties["image"].Value.ToString();
            CloudBlockBlob blob = blobaccessmethods.cloudBlobContainer.GetBlockBlobReference(Path.GetFileName(imagepath));
            blob.UploadFromFile(imagepath, FileMode.Open);

        } 

        ////This happens everytime the Umbraco Application starts
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {

            //// Get the Umbraco Database context
            var db = applicationContext.DatabaseContext.Database;

            //// Check if the DB table does NOT exist
            if (!db.TableExist("Comments"))
            {
                //// Create DB table - and set overwrite to false
                db.CreateTable<Comments>(false);
            }

            //// Check if the DB table does NOT exist
            if (!db.TableExist("Competitions"))
            {
                //// Create DB table - and set overwrite to false
                db.CreateTable<Competitions>(false);
            }

            //// Check if the DB table does NOT exist
            if (!db.TableExist("Competitions2VotingMember"))
            {
                //// Create DB table - and set overwrite to false
                db.CreateTable<Competitions2VotingMember>(false);
            }

            var sectionService = applicationContext.Services.SectionService;

            //Try & find a section with the alias of "mySection"
            var mySection = sectionService.GetSections().SingleOrDefault(x => x.Alias == "MySection");

            //If we can't find the section - doesn't exist
            if (mySection == null)
            {
                sectionService.MakeNew("My Custom Section", "MySection ", "traycontent");

            }

           
        }

       
        ///// <summary>
        ///// Method to create and save the dynamic content from xml
        ///// </summary>
        //public static void CreateSaveDynamicNewsContentFromXmlFeed()
        //{
        //    //// declare the variables

        //    /// <summary>
        //    ///  the content object
        //    /// </summary      
        //    Umbraco.Core.Models.IContent myContent;

        //    /// <summary>
        //    ///  the media object
        //    /// </summary      
        //    Umbraco.Core.Models.IMedia myMedia;

        //    /// <summary>
        //    ///  initialize the content service
        //    /// </summary 
        //    ContentService contentService = (ContentService)ApplicationContext.Current.Services.ContentService;

        //    /// <summary>
        //    ///  initialize the media service
        //    /// </summary 
        //    MediaService mediaService = (MediaService)ApplicationContext.Current.Services.MediaService;

        //    /// <summary>
        //    ///  initialize the content type service
        //    /// </summary 
        //    ContentTypeService contentTypeService = new ContentTypeService(contentService, mediaService);

        //    /// <summary>
        //    ///  initialize the list that contain the model items
        //    /// </summary 
        //    List<umbNewsItemViewModel> mycontentTree = new List<umbNewsItemViewModel>();

        //    /// <summary>
        //    ///  master content id
        //    /// </summary 
        //    int masterContentId;

        //    /// <summary>
        //    ///  parent id
        //    /// </summary 
        //    int parentId;

            
        //    // read the model from the xml file
        //    umbNewsItemViewModel.ReadXmlIntoClass();

        //    mycontentTree = umbNewsItemViewModel.ReadXmlIntoClass();

        //    //publish content from the xml in appropriate news paper websites only

        //    foreach (umbNewsItemViewModel model in mycontentTree)
        //    {
        //        try
        //        {
        //            //get the parent Id dynamically for each website - as in each newspaper
        //            var parentcontent = from pcontent in contentService.GetByLevel(2) where pcontent.ContentType.Alias == "umbNewsOverview" select pcontent;

        //            // get the home page or root content id for each newspaper by the website name as read from the xml
        //            masterContentId = contentService.GetRootContent().FirstOrDefault<IContent>(m => m.Name == model.newsPaperWebsiteName).Id;

        //            // get the parent node id - the News Overview item item below which each  news item has to be published
        //            parentId = parentcontent.FirstOrDefault<IContent>(m => m.ParentId == masterContentId).Id;
        //            myContent = contentService.CreateContent("NewsItem", parentId, contentTypeService.GetContentType("umbNewsItem").Alias, 0);

        //            //populate the content with the class attributes
        //            myContent.CreateDate = model.createdDate;
        //            myContent.Name = model.sectionName;
        //            myContent.WriterId = 0;
                    
        //            //set property values
        //            myContent.SetValue("bodyText", model.story);

        //            if (model.pictureFilePath != null)
        //            {
        //                // 1091 - is hard coded here. It is the id of the incoming media folder created in umbraco back end to store incoming media
        //                // media folder is the same for all the websites/newspaper
        //                // first add the image to the media library
        //                myMedia = mediaService.CreateMedia("TestImage", 1091, "Image", 0);
        //                myMedia.SetValue("umbracoFile", model.pictureFilePath);
        //                mediaService.Save(myMedia, 0, false);

        //                // access the file using file stream
        //                //System.IO.FileStream fileStream = new System.IO.FileStream(model.pictureFilePath, System.IO.FileMode.);
        //                //    myContent.SetValue("image", Tree.ImagePath, fileStream);

                      
        //              //string aName = string.Concat(model.pictureCaption, model.picturefilepath);

        //               MemoryStream uploadFile = new MemoryStream();

        //               using (FileStream fs = System.IO.File.OpenRead(model.pictureFilePath))
        //               {
        //                  fs.CopyTo(uploadFile);
        //               }

        //               HttpPostedFileBase postedFileBase = new MemoryFile(uploadFile, "image", model.pictureFilePath);              
                                                
        //               myContent.SetValue("image", postedFileBase);


        //            }

        //            // just save the content, don't publish
        //            contentService.Save(myContent, 0, false);

        //        }

        //        catch (Exception ex)
        //        {
        //            //log the message using log4net. yet to implement this
        //            LogHelper.Error(typeof(RegisterEvents), ex.Message, ex);
        //            throw new Exception(ex.Message);
        //        }

        //    }

        //}
    }

}
