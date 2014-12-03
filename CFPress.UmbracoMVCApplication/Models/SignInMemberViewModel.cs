// <copyright file="SignInMeberViewModel.cs" company="Clyde and Forth Press">
//    Copyright (c) Clyde and Forth Press. All rights reserved.
// </copyright>
// <Author>Subapriya</Author>
namespace CFPress.UmbracoMVCApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Umbraco.Web.Models;

    /// <summary>
    ///  Register member model
    /// </summary>
    public class SignInMemberViewModel 
    {
        //[DisplayName("Email address")]
        //[Required(ErrorMessage = "Please enter your email address")]
        //[EmailAddress(ErrorMessage = "Please enter a valid email address")]
        //public string EmailAddress { get; set; }
        [DisplayName("User Name")]
        [Required(ErrorMessage = "Please enter your user name")]
        public string UserName { get; set; }
        [UIHint("Password")]
        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
     }
}