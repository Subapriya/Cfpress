using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;
using CFPress.UmbracoMVCApplication.Models;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Logging;
using System.IO;
using CFPress.UmbracoMVCApplication.Utilities;

namespace CFPress.UmbracoMVCApplication.Controllers
{
    public class NewsItemController : BaseController
    {
         /// <summary>
        ///  List to hold the xml tree items
        /// </summary 
        private static List<NewsItemViewModel> mycontentTreeList = new List<NewsItemViewModel>();
            
        /// <summary>
        ///  master content id
        /// </summary 
        private static int masterContentId;

        /// <summary>
        ///  parent id
        /// </summary 
        private static int parentId;

        /// <summary>
        /// content type service
        /// </summary>
        private static ContentTypeService contentTypeService = new ContentTypeService(contentService, mediaService);

        [HttpGet]
        /// <summary>
        /// Method to create and save the dynamic content from xml
        /// </summary>
        public static void CreateSaveDynamicNewsContentFromXmlFeed()
        {
             
           // read the model from the xml file
            BaseController basecontroller = new BaseController();
            NewsItemViewModel.ReadXmlIntoClass();
            mycontentTreeList = NewsItemViewModel.ReadXmlIntoClass();

            //publish content from the xml in appropriate news paper websites only

            foreach (NewsItemViewModel model in mycontentTreeList)
            {
                try
                {
                    //get the parent Id dynamically for each website - as in each newspaper
                    var parentcontent = from pcontent in contentService.GetByLevel(2) where pcontent.ContentType.Alias == "umbNewsOverview" select pcontent;

                    // get the home page or root content id for each newspaper by the website name as read from the xml
                    masterContentId = contentService.GetRootContent().FirstOrDefault<IContent>(m => m.Name == model.newsPaperWebsiteName).Id;

                    // get the parent node id - the News Overview item item below which each  news item has to be published
                    parentId = parentcontent.FirstOrDefault<IContent>(m => m.ParentId == masterContentId).Id;
                    basecontroller.content = contentService.CreateContent("NewsItem", parentId, contentTypeService.GetContentType("umbNewsItem").Alias, 0);

                    //populate the content with the class attributes
                    basecontroller.content.CreateDate = model.createdDate;
                    basecontroller.content.Name = model.sectionName;
                    basecontroller.content.WriterId = 0;

                    //set property values
                    basecontroller.content.SetValue("bodyText", model.story);

                    if (model.pictureFilePath != null)
                    {
                        // 1091 - is hard coded here. It is the id of the incoming media folder created in umbraco back end to store incoming media
                        // media folder is the same for all the websites/newspaper
                        // first add the image to the media library
                        MemoryStream uploadFile = new MemoryStream();
                        using (FileStream fs = System.IO.File.OpenRead(model.pictureFilePath))
                        {
                            fs.CopyTo(uploadFile);
                        }

                        HttpPostedFileBase postedFileBase = new MemoryFile(uploadFile, "image", model.pictureFilePath);
                        System.Drawing.Image image = System.Drawing.Image.FromStream(uploadFile);

                        basecontroller.media = mediaService.CreateMedia("TestImage", 1108, "Image", 0);
                        basecontroller.media.SetValue("umbracoFile", model.pictureFilePath);
                        basecontroller.media.SetValue("umbracoWidth", image.Width);
                        basecontroller.media.SetValue("umbracoHeight", image.Height);
                        basecontroller.media.SetValue("umbracoBytes", uploadFile.Length);
                        mediaService.Save(basecontroller.media, 0, false);

                        // access the file using file stream
                        //System.IO.FileStream fileStream = new System.IO.FileStream(model.pictureFilePath, System.IO.FileMode.);
                        //    myContent.SetValue("image", Tree.ImagePath, fileStream);


                        //string aName = string.Concat(model.pictureCaption, model.picturefilepath);

                       

                        basecontroller.content.SetValue("image", postedFileBase);


                    }

                    // just save the content, don't publish
                    contentService.Save(basecontroller.content, 0, false);

                }

                catch (Exception ex)
                {
                    //log the message using log4net. yet to implement this
                    LogHelper.Error(typeof(NewsItemController), ex.Message, ex);
                    throw new Exception(ex.Message);
                }

            }

        }
    }
}
