using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.ComponentModel;
using System.Web.Mvc;

namespace CFPress.UmbracoMVCApplication.Models
{
    public class RegisterMemberViewModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [DisplayName("Email address")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?",
            ErrorMessage = "Please enter a valid e-mail address")]
        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        //[Remote("CheckEmailIsUsed", "AuthSurface", ErrorMessage = "The email address has already been registered")]
        public string EmailAddress { get; set; }

        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        [UIHint("Password")]
        [DisplayName("Confirm Password")]
        [Required(ErrorMessage = "Please enter your password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Your passwords do not match")]
        public string ConfirmPassword { get; set; }

        public string ReturnUrl { get; set; }

        public string tempGuid { get; set; }
    }


}