using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Services;


namespace CFPress.UmbracoMVCApplication.Utilities
{

    public class CustomUserMembershipProvider : Umbraco.Web.Security.Providers.UsersMembershipProvider
    {
        Umbraco.Core.Services.UserService userService = (UserService)ApplicationContext.Current.Services.UserService;
        public override bool ValidateUser(string username, string password)
        {
            var success = base.ValidateUser(username, password);

            //if (success)
            //{
            //    if (userService.GetByUsername(username).UserType.Name == "Editors")
            //    {
            //        CFPress.UmbracoMVCApplication.Controllers.NewsItemController.CreateSaveDynamicNewsContentFromXmlFeed();
            //    }
            //}
            return success;
        }

    }
}