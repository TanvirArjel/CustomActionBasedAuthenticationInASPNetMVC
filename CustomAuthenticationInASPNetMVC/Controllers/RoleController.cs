using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomAuthenticationInASPNetMVC.DataAccessLayer;
using CustomAuthenticationInASPNetMVC.Models;

namespace CustomAuthenticationInASPNetMVC.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();

        // GET: Roles
        public async Task<ActionResult> RoleList()
        {
            List<Role> roles = await _dbContext.Roles.AsNoTracking().ToListAsync();
            return View(roles);
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> RoleDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public async Task<ActionResult> CreateRole()
        {
            ViewBag.AllActionCategories = await _dbContext.ActionCategories.AsNoTracking().ToListAsync();
            return View();
        }

        // POST: Roles/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRole([Bind(Include = "RoleId,RoleName,Description")] Role role, List<int> selectedActionCategories, List<int> selectedActions)
        {
            if (ModelState.IsValid)
            {
                if (!String.Equals(role.RoleName, "SuperAdmin", StringComparison.CurrentCultureIgnoreCase))
                {
                    if (selectedActionCategories != null)
                    {
                        foreach (var selectedActionCategory in selectedActionCategories)
                        {
                            ActionCategory actionCategory = _dbContext.ActionCategories.Find(selectedActionCategory);
                            role.ActionCategories.Add(actionCategory);
                        }
                    }

                    if (selectedActions != null)
                    {
                        foreach (var selectedAction in selectedActions)
                        {
                            ControllerAction controllerAction = _dbContext.ControllerActions.Find(selectedAction);
                            role.ControllerActions.Add(controllerAction);
                        }
                    }

                }

                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("RoleList");
            }

            ViewBag.AllActionCategories = await _dbContext.ActionCategories.AsNoTracking().ToListAsync();
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> UpdateRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role roletoBeUpdated = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == id);
            if (roletoBeUpdated == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllActionCategories = await _dbContext.ActionCategories.AsNoTracking().ToListAsync();
            return View(roletoBeUpdated);
        }

        // POST: Roles/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateRole(int id, Role role, List<int> selectedActionCategories, List<int> selectedActions)
        {
            if (ModelState.IsValid)
            {
                Role roleToBeUpdated = await _dbContext.Roles.FindAsync(role.RoleId);
                if (roleToBeUpdated != null)
                {
                    if (TryUpdateModel(roleToBeUpdated, "", new string[] { "RoleName", "Description" }))
                    {

                        if (!String.Equals(role.RoleName, "SuperAdmin", StringComparison.CurrentCultureIgnoreCase))
                        {
                            #region UpdatingRoleActionCategories

                            var roleActionCategoriesHs = new HashSet<int>(roleToBeUpdated.ActionCategories.Select(x => x.ActionCategoryId).ToList());
                            var selectedActionCategoriesHs = new HashSet<int>(selectedActionCategories);

                            var roleActionCategoriesToBeDeleted = roleActionCategoriesHs.Except(selectedActionCategoriesHs).ToList();
                            var roleActionCategoriesToBeAdded = selectedActionCategoriesHs.Except(roleActionCategoriesHs).ToList();

                            List<User> roleUsers = await _dbContext.Roles.Where(x => x.RoleId == roleToBeUpdated.RoleId).Select(x => x.Users.ToList()).FirstOrDefaultAsync();

                            foreach (int actionCategoryId in roleActionCategoriesToBeDeleted)
                            {
                                ActionCategory actionCategoryToBeRemoved = roleToBeUpdated.ActionCategories.FirstOrDefault(x => x.ActionCategoryId == actionCategoryId);
                                roleToBeUpdated.ActionCategories.Remove(actionCategoryToBeRemoved);

                                //Delete this action category from the users that contain this role
                                foreach (User rolesUser in roleUsers)
                                {
                                    rolesUser.ActionCategories.Remove(actionCategoryToBeRemoved);
                                }
                            }

                            foreach (int actionCategoryId in roleActionCategoriesToBeAdded)
                            {
                                ActionCategory actionCategory = await _dbContext.ActionCategories.FindAsync(actionCategoryId);
                                roleToBeUpdated.ActionCategories.Add(actionCategory);
                            }

                            #endregion

                            #region UpdatingRoleActions

                            var roleActionsHs = new HashSet<int>(roleToBeUpdated.ControllerActions.Select(x => x.ActionId).ToList());
                            var selectedActionsHs = new HashSet<int>(selectedActions);

                            var roleActionsToBeDeleted = roleActionsHs.Except(selectedActionsHs).ToList();
                            var roleActionsToBeAdded = selectedActionsHs.Except(roleActionsHs).ToList();

                            foreach (int actionId in roleActionsToBeDeleted)
                            {
                                ControllerAction controllerActionToRemove = roleToBeUpdated.ControllerActions.FirstOrDefault(x => x.ActionId == actionId);
                                roleToBeUpdated.ControllerActions.Remove(controllerActionToRemove);

                                //Delete this actions from the users that contain this role

                                foreach (User rolesUser in roleUsers)
                                {
                                    rolesUser.ControllerActions.Remove(controllerActionToRemove);
                                }
                            }

                            foreach (var actionId in roleActionsToBeAdded)
                            {
                                ControllerAction controllerAction = await _dbContext.ControllerActions.FindAsync(actionId);
                                roleToBeUpdated.ControllerActions.Add(controllerAction);

                            }
                            #endregion
                        }

                        await _dbContext.SaveChangesAsync();
                        return RedirectToAction("RoleList");
                    }
                }

                return RedirectToAction("RoleList");
            }
            ViewBag.AllActionCategories = await _dbContext.ActionCategories.ToListAsync();
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> DeleteRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role roleToBeDeleted = await _dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.RoleId == id);
            if (roleToBeDeleted == null)
            {
                return HttpNotFound();
            }
            return View(roleToBeDeleted);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteRoleConfirmed(int id)
        {
            Role roleToBeDeleted = await _dbContext.Roles.FindAsync(id);
            if (roleToBeDeleted != null)
            {
                _dbContext.Roles.Remove(roleToBeDeleted);
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("RoleList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
