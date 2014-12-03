using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFPress.UmbracoMVCApplication.Models
{
        /// <summary>
        /// Reset password View Model
        /// </summary>
                   
         public class ResetPasswordViewModel
        {
            [DisplayName("Email address")]
            [Required(ErrorMessage = "Please enter your email address")]
            [EmailAddress(ErrorMessage = "Please enter a valid email address")]
            public string EmailAddress { get; set; }
            [UIHint("Password")]
            [Required(ErrorMessage = "Please enter your password")]
            public string Password { get; set; }
            [UIHint("Password")]
            [DisplayName("Confirm Password")]
            [Required(ErrorMessage = "Please enter your password")]
            [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Your passwords do not match")]
            public string ConfirmPassword { get; set; }
            public string Guid { get; set; }
        }
    
}