using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomAuthenticationInASPNetMVC.CommonCode;
using CustomAuthenticationInASPNetMVC.DataAccessLayer;
using CustomAuthenticationInASPNetMVC.Models;

namespace CustomAuthenticationInASPNetMVC.Controllers
{
    public class SecurityController : Controller
    {
        private UserDbContext _dbContext = new UserDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateControllerNamesAndActions()
        {
            var controllerNames = GetAllControllersAndActionsName.GetControllerNames();
            HashSet<string> controllerNamesHs = new HashSet<string>(controllerNames);

            foreach (string controllerName in controllerNamesHs)
            {
                
                ActionCategory exActionCategory = _dbContext.ActionCategories.FirstOrDefault(x => x.ActionCategoryName == controllerName);

                List<string> actionNames = GetAllControllersAndActionsName.ActionNames(controllerName);
                HashSet<string> actionNamesHs = new HashSet<string>(actionNames);

                
                if (exActionCategory == null)
                {
                    ActionCategory actionCategory = new ActionCategory
                    {
                        ActionCategoryName = controllerName,
                        Description = "This is a Controller"
                    };

                    _dbContext.ActionCategories.Add(actionCategory);
                    _dbContext.SaveChanges();

                    foreach (string actionName in actionNamesHs)
                    {
                            ControllerAction controllerAction = new ControllerAction()
                            {
                                ActionCategoryId = actionCategory.ActionCategoryId,
                                ActionName = actionName,
                                Description = "This is an action of " + controllerName + " Controller"
                            };
                            _dbContext.ControllerActions.Add(controllerAction);
                      
                    }

                }
                else
                {
                    foreach (string actionName in actionNamesHs)
                    {
                        ControllerAction exControllerAction = _dbContext.ControllerActions.FirstOrDefault(x => x.ActionCategoryId == exActionCategory.ActionCategoryId && x.ActionName == actionName);
                        if (exControllerAction == null)
                        {
                            ControllerAction controllerAction = new ControllerAction()
                            {
                                ActionCategoryId = exActionCategory.ActionCategoryId,
                                ActionName = actionName,
                                Description = "This is an action of " + controllerName + " Controller"
                            };
                            _dbContext.ControllerActions.Add(controllerAction);
                        }
                    }
                }
                
            }

            _dbContext.SaveChanges();

            return View();
        }
    }
}