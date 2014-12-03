// <copyright file="BaseController.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Web;
    using System.Web.Mvc;
    using Umbraco.Core;
    using Umbraco.Core.Services;
    using Umbraco.Web.Mvc;
    using Umbraco.Web;

    /// <summary>
    /// Base class for all the surface controllers in this project
    /// </summary>
    public class BaseController : SurfaceController
    {
        /// <summary>
        ///  the member instance
        /// </summary>    
        internal Umbraco.Core.Models.IMember member;

        /// <summary>
        ///  initialize the member service
        /// </summary>
        internal MemberService memberService;

        /// <summary>
        ///  initialize the member type service
        /// </summary>
        internal MemberTypeService memberTypeService;

        /// <summary>
        ///  the content instance
        /// </summary>
        internal Umbraco.Core.Models.IContent content;

        /// <summary>
        ///  initialize the content service
        /// </summary> 
        internal static ContentService contentService;

        /// <summary>
        ///  the content instance
        /// </summary>
        internal Umbraco.Core.Models.IMedia media;

        /// <summary>
        ///  initialize the media service
        /// </summary>
        internal static MediaService mediaService;

        /// <summary>
        ///  initialize the media service
        /// </summary>
        internal static RelationService relationService;

        /// <summary>
        ///  Umbraco database persistence instance
        /// </summary>
        internal Umbraco.Core.Persistence.Database umbDatabase = ApplicationContext.Current.DatabaseContext.Database;

        /// <summary>
        /// Base controller should have the news paper name
        /// </summary>
        /// <param name="umbracoContext"></param>
        internal string newsPaperWebsiteName;
       
        public BaseController(UmbracoContext umbracoContext) : base(umbracoContext)
        {
            
        }
        public BaseController()
        {
            this.memberService = (MemberService)ApplicationContext.Current.Services.MemberService;
            this.memberTypeService = (MemberTypeService)ApplicationContext.Current.Services.MemberTypeService;
            contentService = (ContentService)ApplicationContext.Current.Services.ContentService;
            mediaService = (MediaService)ApplicationContext.Current.Services.MediaService;
            relationService = (RelationService)ApplicationContext.Current.Services.RelationService;
           
        }
        /// <summary>
        /// MultiButton action attribute 
        /// </summary>
        public class MultiButtonAttribute : ActionNameSelectorAttribute
        {
            public string MatchFormKey { get; set; }
            public string MatchFormValue { get; set; }

            public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
            {
                return controllerContext.HttpContext.Request[MatchFormKey] != null &&
                    controllerContext.HttpContext.Request[MatchFormKey] == MatchFormValue;
            }
        }

        /// <summary>
        /// Method to supply absolute path to the url helper
        /// </summary>
        /// <param name="url"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string AbsoluteAction(UrlHelper url, string actionName, string controllerName, object routeValues)
        {
            string scheme = url.RequestContext.HttpContext.Request.Url.Scheme;

            return url.Action(actionName, controllerName, routeValues, scheme);
        }

      }
}
