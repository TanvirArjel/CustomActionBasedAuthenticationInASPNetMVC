using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CustomActionBasedAuthenticationInASPNetMVC.CommonCode;
using CustomActionBasedAuthenticationInASPNetMVC.DataAccessLayer;
using CustomActionBasedAuthenticationInASPNetMVC.Models;

namespace CustomActionBasedAuthenticationInASPNetMVC.Controllers
{
    public class SecurityController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> UpdateControllerAndActionNames()
        {
            List<string> newControllerNames = GetAllControllerAndActionNames.GetControllerNames2();
            HashSet<string> newControllerNamesHs = new HashSet<string>(newControllerNames);

            List<string> existingControllerNames =  await _dbContext.ActionCategories.AsNoTracking().Select(x => x.ActionCategoryName).ToListAsync();
            HashSet<string> existingControllerNamesHs = new HashSet<string>(existingControllerNames);

            List<string> controllerNamesToBeDeleted = existingControllerNamesHs.Except(newControllerNamesHs).ToList();

            foreach (string controllerName in controllerNamesToBeDeleted)
            {
                ActionCategory controllerToBeDeleted = await _dbContext.ActionCategories.FirstOrDefaultAsync(x => x.ActionCategoryName == controllerName);
                if (controllerToBeDeleted != null)
                {
                    _dbContext.ActionCategories.Remove(controllerToBeDeleted);
                }
                
            }


            foreach (string controllerName in newControllerNamesHs)
            {
                string [] ingnoreControllerNameArray = new [] {"Validation", "ActionCategory", "Action"};
                if (ingnoreControllerNameArray.Contains(controllerName, StringComparer.CurrentCultureIgnoreCase))
                {
                    continue;
                }

                ActionCategory existingActionCategory = await _dbContext.ActionCategories.FirstOrDefaultAsync(x => x.ActionCategoryName == controllerName);

                List<string> newActionNames = GetAllControllerAndActionNames.GetActionNamesByController2(controllerName);
                HashSet<string> newActionNamesHs = new HashSet<string>(newActionNames);

                
                if (existingActionCategory == null)
                {
                    ActionCategory actionCategory = new ActionCategory
                    {
                        ActionCategoryName = controllerName,
                        Description = "This is a Controller"
                    };

                    foreach (string actionName in newActionNamesHs)
                    {
                            ControllerAction controllerAction = new ControllerAction()
                            {
                                ActionCategoryId = actionCategory.ActionCategoryId,
                                ActionName = actionName,
                                Description = "This is an action of " + controllerName + " Controller"
                            };
                        actionCategory.ControllerActions.Add(controllerAction);
                    }

                    _dbContext.ActionCategories.Add(actionCategory);

                }
                else
                {
                    List<string> existingtActionNames = existingActionCategory.ControllerActions.Select(x => x.ActionName).ToList();
                    HashSet<string> existingtActionNamesHs = new HashSet<string>(existingtActionNames);

                    List<string> actionNamesToBeDeleted = existingtActionNamesHs.Except(newActionNamesHs).ToList();

                    foreach (string actionName in actionNamesToBeDeleted)
                    {
                        ControllerAction controlleActionToBeDeleted = _dbContext.ControllerActions.FirstOrDefault(x => x.ActionCategoryId == existingActionCategory.ActionCategoryId && x.ActionName == actionName);
                        if (controlleActionToBeDeleted != null)
                        {
                            _dbContext.ControllerActions.Remove(controlleActionToBeDeleted);
                        }
                        
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

           _dbContext.SaveChanges();

            return View();
        }
    }
}