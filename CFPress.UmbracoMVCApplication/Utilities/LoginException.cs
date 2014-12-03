using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFPress.UmbracoMVCApplication.Utilities
{
    public class LoginException : Exception
    {
        public string errorMessage { get; set; }
        public string errorHeader { get; set; }

        public LoginException(string msg, string header)
        {
            errorMessage = msg;
            errorHeader = header;
        }

        public LoginException()
        {
            errorMessage = this.errorMessage;
            errorHeader = this.errorHeader;

        }
    }
}