using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CFPress.UmbracoMVCApplication.Models;

namespace CFPress.UmbracoMVCApplication.Controllers
{
    public class ForgottenPasswordController : BaseController
    {
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
            //Find the member with the email address
            var findMember = this.memberService.GetByEmail(model.EmailAddress);
            if (findMember != null)
            {
                //We found the member with that email
                //Set expiry date to
                DateTime expiryTime = DateTime.Now.AddMinutes(15);
                //Lets update resetGUID property
                //findMember.getProperty("resetGUID").Value = expiryTime.ToString("ddMMyyyyHHmmssFFFF");
                ////Save the member with the up[dated property value
                //findMember.Save();
                ////Send user an email to reset password with GUID in it
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

        ///// <summary>
        ///// Renders the Forgotten Password view
        ///// @Html.Action("RenderForgottenPassword","AuthSurface");
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult RenderForgottenPassword()
        //{
        //    return PartialView("ForgottenPassword", new ForgottenPasswordViewModel());
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult HandleForgottenPassword(ForgottenPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return PartialView("ForgottenPassword", model);
        //    }
        //    //Find the member with the email address
        //    var findMember = Member.GetMemberFromEmail(model.EmailAddress);
        //    if (findMember != null)
        //    {
        //        //We found the member with that email
        //        //Set expiry date to
        //        DateTime expiryTime = DateTime.Now.AddMinutes(15);
        //        //Lets update resetGUID property
        //        findMember.getProperty("resetGUID").Value = expiryTime.ToString("ddMMyyyyHHmmssFFFF");
        //        //Save the member with the up[dated property value
        //        findMember.Save();
        //        //Send user an email to reset password with GUID in it
        //        EmailHelper email = new EmailHelper();
        //        email.SendResetPasswordEmail(findMember.Email, expiryTime.ToString("ddMMyyyyHHmmssFFFF"));
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("ForgottenPasswordForm.", "No member found");
        //        return PartialView("ForgottenPassword", model);
        //    }
        //    return PartialView("ForgottenPassword", model);
        //}
        ///// <summary>
        ///// Renders the Reset Password View
        ///// @Html.Action("RenderResetPassword","AuthSurface");
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult RenderResetPassword()
        //{
        //    return PartialView("ResetPassword", new ResetPasswordViewModel());
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult HandleResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return PartialView("ResetPassword", model);
        //    }
        //    //Get member from email
        //    var resetMember = Member.GetMemberFromEmail(model.EmailAddress);
        //    //Ensure we have that member
        //    if (resetMember != null)
        //    {
        //        //Get the querystring GUID
        //        var resetQS = Request.QueryString["resetGUID"];
        //        //Ensure we have a vlaue in QS
        //        if (!string.IsNullOrEmpty(resetQS))
        //        {
        //            //See if the QS matches the value on the member property
        //            if (resetMember.getProperty("resetGUID").Value.ToString() == resetQS)
        //            {
        //                //Got a match, now check to see if the 15min window hasnt expired
        //                DateTime expiryTime = DateTime.ParseExact(resetQS, "ddMMyyyyHHmmssFFFF", null);
        //                //Check the current time is less than the expiry time
        //                DateTime currentTime = DateTime.Now;
        //                //Check if date has NOT expired (been and gone)
        //                if (currentTime.CompareTo(expiryTime) < 0)
        //                {
        //                    //Got a match, we can allow user to update password
        //                    resetMember.Password = model.Password;
        //                    //Remove the resetGUID value
        //                    resetMember.getProperty("resetGUID").Value = string.Empty;
        //                    //Save the member
        //                    resetMember.Save();
        //                    return Redirect("/login");
        //                }
        //                else
        //                {
        //                    //ERROR: Reset GUID has expired
        //                    ModelState.AddModelError("ResetPasswordForm.", "Reset GUID has expired");
        //                    return PartialView("ResetPassword", model);
        //                }
        //            }
        //            else
        //            {
        //                //ERROR: QS does not match what is stored on member property
        //                //Invalid GUID
        //                ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
        //                return PartialView("ResetPassword", model);
        //            }
        //        }
        //        else
        //        {
        //            //ERROR: No QS present
        //            //Invalid GUID
        //            ModelState.AddModelError("ResetPasswordForm.", "Invalid GUID");
        //            return PartialView("ResetPassword", model);
        //        }
        //    }
        //    return PartialView("ResetPassword", model);
        //}


    }
}
