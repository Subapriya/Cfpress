// <copyright file="SignInMemberController.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using umbraco;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;
    using Umbraco.Web;
    using Umbraco.Web.Mvc;
    using CFPress.UmbracoMVCApplication.Models;
    using CFPress.UmbracoMVCApplication.Pocos;
    using CFPress.UmbracoMVCApplication.Utilities;
    using umbraco.cms.businesslogic.member;
    using System.Web.Security;
    using Umbraco.Web.Security.Providers;
    /// <summary>
    /// Controller class to handle new member registration
    /// </summary>
    public class SignInMemberController : BaseController
    {
        MembersMembershipProvider membersmembershipprovider = new MembersMembershipProvider();
        CustomUserMembershipProvider usermp = new CustomUserMembershipProvider();
        
        /// <summary>
        ///  Get method to handle basic form load
        /// </summary>
        /// <param name="model">Register member model instance</param>
        /// <returns>Register member partial view</returns>
        public ActionResult Index()
        {
            SignInMemberViewModel model = new SignInMemberViewModel();
            if (string.IsNullOrEmpty(HttpContext.Request["ReturnUrl"]))
            {
                //If returnURL is empty then set it to /
                model.ReturnUrl = "/";
            }
            else
            {
                //Lets use the return URL in the querystring or form post
                model.ReturnUrl = HttpContext.Request["ReturnUrl"];
            }

            return this.PartialView("SignInMember");
        }

        /// <summary>
        /// Post method to handle login
        /// </summary>
        /// <param name="model">Model instance</param>
        /// <returns>Returns the appropriate view</returns>
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Login")]
        public ActionResult HandleLogin(SignInMemberViewModel model)
        {
            //// Check if the data on the model is valid
            if (!ModelState.IsValid)
            {
                //// There was a validation error with the data
                return this.PartialView("SignInMember", model);
            }

            try
            {
                //// try logging in the member, this only does forms authentication

                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    //// Get the member from their email address
                    var checkMember = memberService.GetByUsername(model.UserName);
                    //Check the member exists
                    if (checkMember != null)
                    {
                        //Let's check they have verified their email address
                        if (Convert.ToBoolean(checkMember.GetValue("hasemailverified")))
                        {
                            //Update number of logins counter
                            int noLogins = 0;

                            if (int.TryParse(checkMember.GetValue("numberoflogins").ToString(), out noLogins))
                            {
                                //// no need to do anything. Just getting the value of number of logins in to the variable
                            }

                            //Update the counter
                            checkMember.SetValue("numberoflogins", noLogins);
                            //Update label with last login date to now
                            checkMember.LastLoginDate = DateTime.Now;

                            // //Update label with last logged in IP address & Host Name
                            //string hostName = Dns.GetHostName();
                            //string clientIPAddress = Dns.GetHostAddresses(hostName).GetValue(0).ToString();
                            //checkMember.getProperty("hostNameOfLastLogin").Value = hostName;
                            //checkMember.getProperty("iPofLastLogin").Value = clientIPAddress;
                            //Save the details
                            memberService.Save(checkMember);
                            //If they have verified then lets log them in
                            //Set Auth cookie
                            FormsAuthentication.SetAuthCookie(model.UserName, true);
                            model.ReturnUrl = "/";
                            //Once logged in - redirect them back to the return URL
                            return new RedirectResult(model.ReturnUrl);
                            //return this.PartialView("SignInMember", model);
                        }
                        else
                        {
                            ModelState.AddModelError("SignInForm.", "Email not verified");
                            return this.PartialView("SignInMember", model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("SignInForm.", "Member not found");
                        return this.PartialView("SignInMember", model);
                    }
                }
                else
                {
                    ModelState.AddModelError("SignInForm.", "Invalid details");
                    return this.PartialView("SignInMember", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Login Failed", "Sign In member form." + ex.Message);
                return this.PartialView("SignInMember", model);
            }
        }

        /// <summary>
        /// Post method to handle logout
        /// </summary>
        /// <param name="model">Model instance</param>
        /// <returns>Returns the appropriate view</returns>
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Logout")]
        public ActionResult HandleLogout(SignInMemberViewModel model)
        {
            //// No need to check for model validity here because the password and username need not be
            //// populated in the model to log out the current logged in member
            //// If the member is already logged in, return a message to show logout button
            if (Members.IsLoggedIn())
            {
                Members.Logout();
            }

            return this.RedirectToCurrentUmbracoPage();
        }

        /// <summary>
        /// Renders the Forgotten Password view
        /// @Html.Action("RenderForgottenPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RenderForgottenPassword()
        {
            return PartialView("ForgottenPassword");
        }

        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Remind Me")]
        public ActionResult HandleForgottenPassword(ForgottenPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ForgottenPassword", model);
            }
            else
            {
                //Find the member with the email address
                var findMember = this.memberService.GetByEmail(model.EmailAddress);
                if (findMember != null)
                {
                    //We found the member with that email
                    //Set expiry date to
                    DateTime expiryTime = DateTime.Now.AddMinutes(15);
                    //Lets update resetGUID property
                    findMember.SetValue(("resetGUID"), expiryTime.ToString("ddMMyyyyHHmmssFFFF"));
                    memberService.Save(findMember);
                    ResetPasswordViewModel resetpasswordmodel = new ResetPasswordViewModel();
                    resetpasswordmodel.Guid = findMember.GetValue("resetGUID").ToString();
                    resetpasswordmodel.EmailAddress = model.EmailAddress;
                    ////Send user an email to reset password with GUID in it
                    try
                    {
                        new MailController().ResetPasswordRequest(resetpasswordmodel).Deliver();
                        ViewData.Add("ForgottenPasswordEmailResult", "The link to reset password sent to your email address");
                    }
                    catch(Exception ex)
                    {
                        ViewData.Add("ForgottenPasswordEmailResult", "Send email failed");
                    }

                }
                else
                {
                    ModelState.AddModelError("ForgottenPasswordForm.", "No member registered with this email address");
                    return PartialView("ForgottenPassword", model);
                }
                return PartialView("ForgottenPassword", model);
            }
        }


        /// <summary>
        /// Renders the Reset Password View
        /// @Html.Action("RenderResetPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderResetPassword()
        {
            return PartialView("ResetPassword", new ResetPasswordViewModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [MultiButton(MatchFormKey = "action", MatchFormValue = "Reset Password")]
        public ActionResult HandleResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ResetPassword", model);
            }
            //Get member from email
            var resetMember = memberService.GetByEmail(model.EmailAddress);
            //Ensure we have that member
            if (resetMember != null)
            {
                //Get the querystring GUID
                var resetQS = model.Guid;
                //Ensure we have a vlaue in QS
                if (!string.IsNullOrEmpty(resetQS))
                {
                    //See if the QS matches the value on the member property
                    if (resetMember.GetValue("resetGUID").ToString() == resetQS)
                    {
                        //Got a match, now check to see if the 15min window hasnt expired
                        DateTime expiryTime = DateTime.ParseExact(resetQS, "ddMMyyyyHHmmssFFFF", null);
                        //Check the current time is less than the expiry time
                        DateTime currentTime = DateTime.Now;
                        //Check if date has NOT expired (been and gone)
                        if (currentTime.CompareTo(expiryTime) < 0)
                        {
                            
                            //Remove the resetGUID value
                            resetMember.SetValue("resetGUID",string.Empty);
                            //Save the member
                            memberService.Save(resetMember);
                            //Got a match, we can allow user to update password
                            memberService.SavePassword(resetMember, model.Password);
                            return Redirect("/sign-in");
                        }
                        else
                        {
                            //ERROR: Reset GUID has expired
                            ModelState.AddModelError("ResetPasswordForm.", "Reset GUID has expired");
                            return PartialView("ResetPassword", model);
                        }
                    }
                    else
                    {
                        //ERROR: model guid does not match what is stored on member property
                        //Invalid GUID
                        ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
                        return PartialView("ResetPassword", model);
                    }
                }
                else
                {
                    //ERROR: No model guid property present
                    //Invalid GUID
                    ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
                    return PartialView("ResetPassword", model);
                }
            }
            return PartialView("ResetPassword", model);
        }

    }
}