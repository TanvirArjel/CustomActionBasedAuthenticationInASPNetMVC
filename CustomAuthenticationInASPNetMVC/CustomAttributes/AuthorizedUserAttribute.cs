using System;
using System.Collections.Generic;
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
        //private string _roles;
        //public string UserRole
        //{
        //    get
        //    {
        //        return this._roles;
        //    }

        //    set
        //    {
        //        _roles = value;
        //    }
        //}
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    var user = HttpContext.Current.Session["LoggedInUser"] as User;

        //    var userName = user.UserName;

        //    UserRepository _userRepository = new UserRepository();

        //    return _userRepository.IsUserInRole(userName, this.UserRole);

        //    if (user == null)
        //    {
        //        httpContext.Result = new RedirectToRouteResult(new RouteValueDictionary
        //        (new
        //        {
        //            Controller = "Users",
        //            Action = "UserLogin",
        //            returnUrl = httpContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
        //        }));
        //    }
        //}
        //public AuthorizedUserAttribute(string roles)
        //{
        //    this.UserRoles = roles;
        //}

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = HttpContext.Current.Session["LoggedInUser"] as User;

            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                (new
                {
                    Controller = "Users",
                    Action = "UserLogin",
                    returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
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
                        returnUrl = filterContext.HttpContext.Request.Url.GetComponents(UriComponents.PathAndQuery, UriFormat.SafeUnescaped)
                    }));
                }
            }


        }
    }
}