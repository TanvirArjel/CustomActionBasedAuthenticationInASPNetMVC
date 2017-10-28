using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomAuthenticationInASPNetMVC.DataAccessLayer;

namespace CustomAuthenticationInASPNetMVC.Controllers
{
    public class ValidationController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();
        public JsonResult IsUserNameAlreadyExists(string userName, int? userId)
        {
            bool uerNameCheckingStatus = _dbContext.Users.Any(x => x.UserName == userName && x.UserId != userId);
            return Json(!uerNameCheckingStatus, JsonRequestBehavior.AllowGet);
        }
    }
}