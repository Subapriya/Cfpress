using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionMailer.Net.Mvc4;
using CFPress.UmbracoMVCApplication.Models;
using SendGrid;

namespace CFPress.UmbracoMVCApplication.Controllers
{
    public class MailController : MailerBase 
    {

        public EmailResult ResetPasswordRequest(ResetPasswordViewModel model)
        {
            From = string.Format("{0} <{1}>", "Sender's name", "snageswaran@cfpress.co.uk");
            To.Add(model.EmailAddress);
            Subject = "Reset password request";
            return Email("ResetPasswordRequest",model);
        }

        public EmailResult RegisterMemberVerificationEmail(RegisterMemberViewModel model)
        {
            From = string.Format("{0} <{1}>", "Sender's name", "snageswaran@cfpress.co.uk");
            To.Add(model.EmailAddress);
            Subject = "Verify email address";
            return Email("RegisterMemberVerificationEmail", model);
        }

        public void SendEmail()
        {
            SendGridMessage message = new SendGridMessage();
            
        }
    }

}
