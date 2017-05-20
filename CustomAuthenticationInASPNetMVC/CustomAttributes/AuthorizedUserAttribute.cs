using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CustomAuthenticationInASPNetMVC.Models;
using CustomAuthenticationInASPNetMVC.Repository;

namespace CustomAuthenticationInASPNetMVC.CustomAttributes
{

    
    public class AuthorizedUserAttribute : AuthorizeAttribute
    {
        
        public string UserRoles { get; set; }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SkipAuthorization(filterContext)) return;

            var user = HttpContext.Current.Session["LoggedInUser"] as User;

            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                (new
                {
                    Controller = "Users",
                    Action = "UserLogin",
                    //returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                }));
            }
            else
            {
                var userName = user.UserName;

                UserRepository _userRepository = new UserRepository();

                bool isUserInRole = _userRepository.IsUserInRole(userName, UserRoles);
                if (!isUserInRole)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    (new
                    {
                        Controller = "Users",
                        Action = "UserLogin",
                        //returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                    }));
                }
            }
        }


        private static bool SkipAuthorization(AuthorizationContext filterContext)
        {
            Contract.Assert(filterContext != null);

            return filterContext.ActionDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any()
                   || filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();
        }

    }
}