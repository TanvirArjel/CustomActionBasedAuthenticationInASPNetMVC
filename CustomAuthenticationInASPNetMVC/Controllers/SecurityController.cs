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
        private readonly UserDbContext _dbContext = new UserDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateControllerNamesAndActions()
        {
            List<string> newControllerNames = GetAllControllerAndActionNames.GetControllerNames2();
            HashSet<string> newControllerNamesHs = new HashSet<string>(newControllerNames);

            List<string> existingControllerNames = _dbContext.ActionCategories.Select(x => x.ActionCategoryName).ToList();
            HashSet<string> existingControllerNamesHs = new HashSet<string>(existingControllerNames);

            List<string> controllerNamesToBeDeleted = existingControllerNamesHs.Except(newControllerNamesHs).ToList();

            foreach (string controllerName in controllerNamesToBeDeleted)
            {
                ActionCategory controllerToBeDeleted = _dbContext.ActionCategories.FirstOrDefault(x => x.ActionCategoryName == controllerName);
                if (controllerToBeDeleted == null)
                {
                    continue;
                }
                _dbContext.ActionCategories.Remove(controllerToBeDeleted);
            }


            foreach (string controllerName in newControllerNamesHs)
            {
                
                ActionCategory existingActionCategory = _dbContext.ActionCategories.FirstOrDefault(x => x.ActionCategoryName == controllerName);

                List<string> newActionNames = GetAllControllerAndActionNames.GetActionNamesByController2(controllerName);
                HashSet<string> newActionNamesHs = new HashSet<string>(newActionNames);

                
                if (existingActionCategory == null)
                {
                    ActionCategory actionCategory = new ActionCategory
                    {
                        ActionCategoryName = controllerName,
                        Description = "This is a Controller"
                    };

                    _dbContext.ActionCategories.Add(actionCategory);
                    _dbContext.SaveChanges();

                    foreach (string actionName in newActionNamesHs)
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
                    List<string> existingtActionNames = existingActionCategory.ControllerActions.Select(x => x.ActionName).ToList();
                    HashSet<string> existingtActionNamesHs = new HashSet<string>(existingtActionNames);

                    List<string> actionNamesToBeDeleted = existingtActionNamesHs.Except(newActionNamesHs).ToList();

                    foreach (string actionName in actionNamesToBeDeleted)
                    {
                        ControllerAction controlleActionToBeDeleted = _dbContext.ControllerActions.FirstOrDefault(x => x.ActionCategoryId == existingActionCategory.ActionCategoryId && x.ActionName == actionName);
                        if (controlleActionToBeDeleted == null)
                        {
                            continue;
                        }
                        _dbContext.ControllerActions.Remove(controlleActionToBeDeleted);
                    }

                    List<string> actionNamesToBeAdded = newActionNamesHs.Except(existingtActionNamesHs).ToList();

                    foreach (string actionName in actionNamesToBeAdded)
                    {
                        ControllerAction controllerAction = new ControllerAction()
                        {
                            ActionCategoryId = existingActionCategory.ActionCategoryId,
                            ActionName = actionName,
                            Description = "This is an action of " + controllerName + " Controller"
                        };
                        _dbContext.ControllerActions.Add(controllerAction);
                    }
                }
                
            }

           // _dbContext.SaveChanges();

            return View();
        }
    }
}