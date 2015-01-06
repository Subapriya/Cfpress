using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using CFPress.UmbracoMVCApplication.Models;
using System.Net.Mail;
using Umbraco.Core.Services;
using Umbraco.Core;
using SendGrid;
using ActionMailer.Net.Mvc4;
using System.Net;

namespace CFPress.UmbracoMVCApplication.Controllers
{
    public class RegisterMemberController : BaseController
    {
        /// <summary>
        ///  the content object
        /// </summary      
        Umbraco.Core.Models.IMember myMember;

        public ActionResult Index()
        {
            return PartialView("RegisterMember");
        }

        [HttpPost]
        public ActionResult HandleRegister(RegisterMemberViewModel model)
        {
            if (!ModelState.IsValid)
                return this.PartialView("RegisterMember");

            // before adding the member check if the same email address exists
            else if (memberService.GetByEmail(model.EmailAddress) == null)
            {
                try
                {
                    myMember = memberService.CreateMember(model.Name, model.EmailAddress, model.Name, memberTypeService.Get("Member"));

                    // we can set the hasverified email property to true as of now
                    // once email generation is confirmed, this can be changed
                   
                    myMember.SetValue("hasemailverified", false);
                    memberService.Save(myMember, false);
                    memberService.SavePassword(myMember, model.Password);


                    //Create temporary GUID
                    var tempGUID = Guid.NewGuid();
                    model.tempGuid = tempGUID.ToString();                 
                    if (myMember != null)
                    {
                        //Set the verification email GUID value on the member
                        myMember.SetValue("emailverifyGUID", tempGUID.ToString());
                        //// Set the registered newspaper name
                        string mastercontentname = contentService.GetParent(CurrentPage.Id).Name;
                        myMember.SetValue("registerednewspaper", mastercontentname);
                        //Set the Joined Date label on the member
                        // we can set other properties that we want to set here
                        //myMember.SetValue("joinedDate",DateTime.Now.ToString("dd/MM/yyyy @ HH:mm:ss"));
                        //Save changes
                        memberService.Save(myMember);
                    }

                    try
                    {
                        //Send out verification email, with GUID in it

                        EmailResult emailresult = new MailController().RegisterMemberVerificationEmail(model);
                        SendGridMessage message = new SendGridMessage();
                        
                        // Create credentials, specifying your user name and password.
                        var credentials = new NetworkCredential("azure_cbedf55269e3533b5976a8061ad59d69@azure.com", "tX8RE8oO63Z1wtZ");

                        // Create a Web transport for sending email.
                        var transportWeb = new Web(credentials);

                        // Send the email.
                        // You can also use the **DeliverAsync** method, which returns an awaitable task.
                        transportWeb.Deliver(message);
                        
                    }
                    catch(Exception ex)
                    {
                        // log the email generation error
                    }
                   
                    // return to the login page

                    return new RedirectResult("/sign-in");
                }
                catch(Exception ex)
                {
                   // log the error
                    return CurrentUmbracoPage();
                }
            }

            else
            {
                ModelState.AddModelError("Registration Failed", "Email address already exists");
                return PartialView("RegisterMember", model);
            }


        }


        /// <summary>
        /// Renders the Verify Email
        /// @Html.Action("RenderVerifyEmail","RegisterMemberSurface");
        /// </summary>
        /// <returns></returns>
        public ActionResult RenderVerifyEmail(string verifyGUID)
        {
            //Auto binds and gets guid from the querystring
             myMember = memberService.GetAllMembers().SingleOrDefault(x => x.GetValue("emailverifyGUID").ToString() == verifyGUID);
            //Ensure we find a member with the verifyGUID
             if (myMember != null)
            {
                //We got the member, so let's update the verify email checkbox
                myMember.SetValue("hasemailverified", true);
                //Save the member
                memberService.Save(myMember);
            }
            else
            {
                //Couldn't find them - most likely invalid GUID
                return Redirect("/");
            }
            
            return Redirect("/");
        }

       
    }
}
