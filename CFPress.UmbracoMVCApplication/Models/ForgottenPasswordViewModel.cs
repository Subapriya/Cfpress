﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CFPress.UmbracoMVCApplication.Models
{
        //Forgotten Password View Model
        public class ForgottenPasswordViewModel
        {
            [DisplayName("Email address")]
            [Required(ErrorMessage = "Please enter your email address")]
            [EmailAddress(ErrorMessage = "Please enter a valid email address")]
            public string EmailAddress { get; set; }
        }
    
}