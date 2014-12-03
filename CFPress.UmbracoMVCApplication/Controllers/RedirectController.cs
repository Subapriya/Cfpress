// <copyright file="RedirectController.cs" company="Clyde and Forth Press">
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
    using CFPress.UmbracoMVCApplication.Models;

    /// <summary>
    /// Controller class to handle new member registration
    /// </summary>
    public class RedirectController : BaseController
    {
        /// <summary>
        ///  the content object
        /// </summary      
        Umbraco.Core.Models.IMember myMember;

        /// <summary>
        ///  initialize the member service
        /// </summary 
        MemberService memberService = (MemberService)ApplicationContext.Current.Services.MemberService;

        /// <summary>
        ///  initialize the member type service
        /// </summary 
        MemberTypeService memberTypeService = (MemberTypeService)ApplicationContext.Current.Services.MemberTypeService;

        /// <summary>
        /// Instance to store the nodeId
        /// </summary>
        private int nodeId;

        /// <summary>
        /// Get method to redirect to register page
        /// </summary>
        /// <returns>Redirect to register page</returns>
        [HttpGet]
        public ActionResult HandleSubmittoRegister()
        {
            //// getting the node by url. If the url changes the code must be changed
            this.nodeId = uQuery.GetNodeByUrl("/register").Id;
            RegisterMemberViewModel model = new RegisterMemberViewModel();
            model.ReturnUrl = Request.UrlReferrer.ToString();
            //return this.RedirectToUmbracoPage(this.nodeId);
            return this.PartialView("RegisterMember", model);
            //return this.RedirectToUmbracoPage(this.nodeId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleRegister(RegisterMemberViewModel model)
        {
            if (!ModelState.IsValid)
                return this.PartialView("RegisterMember");

            // before adding the member check if the same email address exists
            else if (memberService.GetByEmail(model.EmailAddress) == null)
            {
                myMember = memberService.CreateMember(model.Name, model.EmailAddress, model.Name, memberTypeService.Get("Member"));

                // if there are other properties like address, phone those can be added as properties
                // newMember.getProperty(“address”).Value = txtAddress.Text; //set value of property with alias address

                // we can set the hasverified email property to true as of now
                // once email generation is confirmed, this can be changed
                //new MailController().RegisterMemberVerificationEmail(model.EmailAddress).Deliver();
                myMember.SetValue("hasemailverified", true);
                memberService.Save(myMember, false);
                memberService.SavePassword(myMember, model.Password);
                ////redirect to add user/registration form
                ////return RedirectToCurrentUmbracoPage();
                return new RedirectResult(model.ReturnUrl);
            }

            else
            {
                ModelState.AddModelError("Registration Failed", "Email address already exists");
                return PartialView("RegisterMember", model);
            }

        }

        /// <summary>
        ///  Get method to redirect to sign in page
        /// </summary>
        /// <returns>Redirect to the sign in page</returns>
        [HttpGet]
        public ActionResult HandleSubmittoLogin()
        {
            //// get the url for the sign in page
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return PartialView("Comments");
            }
            else
            {
                this.nodeId = uQuery.GetNodeByUrl("/sign-in").Id;
                return this.RedirectToUmbracoPage(this.nodeId);
            }
        }
      }
}
