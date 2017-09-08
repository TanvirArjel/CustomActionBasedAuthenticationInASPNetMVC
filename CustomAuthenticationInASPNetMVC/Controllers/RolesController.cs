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
    public class RolesController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();

        // GET: Roles
        public async Task<ActionResult> Index()
        {
            return View(await _dbContext.Roles.ToListAsync());
        }

        // GET: Roles/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            ViewBag.AllActionCategories = _dbContext.ActionCategories.ToList();
            return View();
        }

        // POST: Roles/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RoleId,RoleName,Description")] Role role, List<int> selectedActionCategories, List<int> selectedActions)
        {
            if (ModelState.IsValid)
            {
                if (!String.Equals(role.RoleName,"SuperAdmin",StringComparison.CurrentCultureIgnoreCase))
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
                return RedirectToAction("Index");
            }

            ViewBag.AllActionCategories = _dbContext.ActionCategories.ToList();
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }

            //ViewBag.AllActions = _dbContext.ControllerActions.ToList();
            ViewBag.AllActionCategories = _dbContext.ActionCategories.ToList();
            return View(role);
        }

        // POST: Roles/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RoleId,RoleName,Description")] Role role, List<int> selectedActionCategories, List<int> selectedActions)
        {
            if (ModelState.IsValid)
            {

                var roleToBeUpdated = _dbContext.Roles.Find(role.RoleId);
                roleToBeUpdated.RoleName = role.RoleName;
                roleToBeUpdated.Description = role.Description;
                roleToBeUpdated.ActionCategories.Clear();
                roleToBeUpdated.ControllerActions.Clear();

                if (!String.Equals(role.RoleName,"SuperAdmin",StringComparison.CurrentCultureIgnoreCase))
                {
                    if (selectedActionCategories != null)
                    {
                        foreach (var selectedActionCategory in selectedActionCategories)
                        {
                            ActionCategory actionCategory = _dbContext.ActionCategories.Find(selectedActionCategory);
                            roleToBeUpdated.ActionCategories.Add(actionCategory);
                        }
                    }

                    if (selectedActions != null)
                    {
                        foreach (var selectedAction in selectedActions)
                        {
                            ControllerAction action = _dbContext.ControllerActions.Find(selectedAction);
                            roleToBeUpdated.ControllerActions.Add(action);
                        }
                    }
                }

                _dbContext.Entry(roleToBeUpdated).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AllActionCategories = _dbContext.ActionCategories.ToList();
            return View(role);
        }

        // GET: Roles/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = await _dbContext.Roles.FindAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Role role = await _dbContext.Roles.FindAsync(id);
            _dbContext.Roles.Remove(role);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
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
