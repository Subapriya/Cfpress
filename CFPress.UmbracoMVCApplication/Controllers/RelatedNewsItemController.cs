// <copyright file="RelatedNewsItemController.cs" company="Clyde and Forth Press">
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
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;
    using Umbraco.Web.Mvc;
    using CFPress.UmbracoMVCApplication.Models;

    public class RelatedNewsItemController : BaseController
    {
        /// <summary>
        ///  Relation service instance
        /// </summary>
        private RelationService relationService = (RelationService)ApplicationContext.Current.Services.RelationService;

        /// <summary>
        ///  the relation object
        /// </summary      
        private IEnumerable<Umbraco.Core.Models.IRelation> relation;

        /// <summary>
        ///  Umbraco helper instance
        /// </summary>
        private Umbraco.Web.UmbracoHelper umbracoHelper = new Umbraco.Web.UmbracoHelper();

        [HttpGet]
        public ActionResult GetRelatedPagesList(RelatedNewsItemViewModel model)
        {
            relation = relationService.GetByParentId(CurrentPage.Id).AsEnumerable();

            var contentCollection = Umbraco.ContentQuery.Content(relation.Select(p => p.ChildId));
            model.relatedNewsItemList = contentCollection;
            return PartialView("RelatedPages", model);
        }
    }
}
