using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using umbraco.cms.businesslogic.member;
using UmbracoMVCApplication.Models;

namespace UmbracoMVCApplication.Controllers
{
    public class AuthSurfaceController : BaseController
    {
        /// <summary>
        /// Renders the Login view
        /// @Html.Action("RenderLogin","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderLogin()
        {
            AuthViewModel.LoginViewModel loginModel = new AuthViewModel.LoginViewModel();
            if (string.IsNullOrEmpty(HttpContext.Request["ReturnUrl"]))
            {
                //If returnURL is empty then set it to /
                loginModel.ReturnUrl = "/";
            }
            else
            {
                //Lets use the return URL in the querystring or form post
                loginModel.ReturnUrl = HttpContext.Request["ReturnUrl"];
            }
            return PartialView("Login", loginModel);
        }
   
        /// <summary>
        /// Renders the Forgotten Password view
        /// @Html.Action("RenderForgottenPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderForgottenPassword()
        {
            return PartialView("ForgottenPassword", new AuthViewModel.ForgottenPasswordViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleForgottenPassword(AuthViewModel.ForgottenPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ForgottenPassword", model);
            }
            //Find the member with the email address
            var findMember = this.memberService.GetByEmail(model.EmailAddress);
            if (findMember != null)
            {
                //We found the member with that email
                //Set expiry date to
                DateTime expiryTime = DateTime.Now.AddMinutes(15);
                //Lets update resetGUID property
                //findMember.("resetGUID").Value = expiryTime.ToString("ddMMyyyyHHmmssFFFF");
                //Save the member with the up[dated property value
                memberService.Save(findMember);
                //Send user an email to reset password with GUID in it
                //EmailHelper email = new EmailHelper();
                //email.SendResetPasswordEmail(findMember.Email, expiryTime.ToString("ddMMyyyyHHmmssFFFF"));
            }
            else
            {
                ModelState.AddModelError("ForgottenPasswordForm.", "No member found");
                return PartialView("ForgottenPassword", model);
            }
            return PartialView("ForgottenPassword", model);
        }
        /// <summary>
        /// Renders the Reset Password View
        /// @Html.Action("RenderResetPassword","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderResetPassword()
        {
            return PartialView("ResetPassword", new AuthViewModel.ResetPasswordViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleResetPassword(AuthViewModel.ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("ResetPassword", model);
            }
            //Get member from email
            var resetMember = this.memberService.GetByEmail(model.EmailAddress);
            //Ensure we have that member
            if (resetMember != null)
            {
                //Get the querystring GUID
                var resetQS = Request.QueryString["resetGUID"];
                //Ensure we have a vlaue in QS
                if (!string.IsNullOrEmpty(resetQS))
                {
                    //See if the QS matches the value on the member property
                    if (resetMember.getProperty("resetGUID").Value.ToString() == resetQS)
                    {
                        //Got a match, now check to see if the 15min window hasnt expired
                        DateTime expiryTime = DateTime.ParseExact(resetQS, "ddMMyyyyHHmmssFFFF", null);
                        //Check the current time is less than the expiry time
                        DateTime currentTime = DateTime.Now;
                        //Check if date has NOT expired (been and gone)
                        if (currentTime.CompareTo(expiryTime) < 0)
                        {
                            //Got a match, we can allow user to update password
                            resetMember.Password = model.Password;
                            //Remove the resetGUID value
                            resetMember.getProperty("resetGUID").Value = string.Empty;
                            //Save the member
                            resetMember.Save();
                            return Redirect("/login");
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
                        //ERROR: QS does not match what is stored on member property
                        //Invalid GUID
                        ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
                        return PartialView("ResetPassword", model);
                    }
                }
                else
                {
                    //ERROR: No QS present
                    //Invalid GUID
                    ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
                    return PartialView("ResetPassword", model);
                }
            }
            return PartialView("ResetPassword", model);
        }
        /// <summary>
        /// Renders the Register View
        /// @Html.Action("RenderRegister","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderRegister()
        {
            return PartialView("Register", new AuthViewModel.RegisterViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleRegister(AuthViewmodel.RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("Register", model);
            }
            //Member Type
            MemberType umbJobMemberType = this.
            //Umbraco Admin User (The Umbraco back office username who will create the member via the API)
            User umbUser = new User("Admin");
            //Model valid let's create the member
            try
            {
                Member createMember = Member.MakeNew(model.Name, model.EmailAddress, model.EmailAddress, umbJobMemberType, umbUser);
                //Set password on the newly created member
                createMember.Password = model.Password;
                //Set the verified email to false
                createMember.getProperty("hasVerifiedEmail").Value = false;
                //Set the profile URL to be the member ID, so they have a unqie profile ID, until they go to set it
                createMember.getProperty("profileURL").Value = createMember.Id;
                //Save the changes
                createMember.Save();
            }
            catch (Exception ex)
            {
                //EG: Duplicate email address - already exists
                throw;
            }
            //Create temporary GUID
            var tempGUID = Guid.NewGuid();
            //Fetch our new member we created by their email
            var updateMember = Member.GetMemberFromEmail(model.EmailAddress);
            //Just to be sure...
            if (updateMember != null)
            {
                //Set the verification email GUID value on the member
                updateMember.getProperty("emailVerifyGUID").Value = tempGUID.ToString();
                //Set the Joined Date label on the member
                updateMember.getProperty("joinedDate").Value = DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss");
                //Save changes
                updateMember.Save();
            }
            //Send out verification email, with GUID in it
            EmailHelper email = new EmailHelper();
            email.SendVerifyEmail(model.EmailAddress, tempGUID.ToString());
            //Return the view...
            return PartialView("Register", new RegisterViewModel());
        }
        /// <summary>
        /// Renders the Verify Email
        /// @Html.Action("RenderVerifyEmail","AuthSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderVerifyEmail(string verifyGUID)
        {
            //Auto binds and gets guid from the querystring
            Member findMember = Member.GetAllAsList().SingleOrDefault(x => x.getProperty("emailVerifyGUID").Value.ToString() == verifyGUID);
            //Ensure we find a member with the verifyGUID
            if (findMember != null)
            {
                //We got the member, so let's update the verify email checkbox
                findMember.getProperty("hasVerifiedEmail").Value = true;
                //Save the member
                findMember.Save();
            }
            else
            {
                //Couldn't find them - most likely invalid GUID
                return Redirect("/");
            }
            //Just in case...
            return Redirect("/");
        }
        //REMOTE Validation
        /// <summary>
        /// Used with jQuery Validate to check when user registers that email address not already used
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public JsonResult CheckEmailIsUsed(string emailAddress)
        {
            //Try and get member by email typed in
            var checkEmail = Member.GetMemberFromEmail(emailAddress);
            if (checkEmail != null)
            {
                return Json(String.Format("The email address '{0}' is already in use.", emailAddress), JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
             
}
