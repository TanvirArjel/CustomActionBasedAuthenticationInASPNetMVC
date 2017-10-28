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
    public class ActionCategoryController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();

        // GET: Sections
        public async Task<ActionResult> ActionCategoryList()
        {
            List<ActionCategory> actionCategories = await _dbContext.ActionCategories.ToListAsync();
            return View(actionCategories);
        }

        // GET: Sections/Details/5
        public async Task<ActionResult> ActionCategoryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionCategory actionCategory = await _dbContext.ActionCategories.FindAsync(id);
            if (actionCategory == null)
            {
                return HttpNotFound();
            }
            return View(actionCategory);
        }

        // GET: Sections/Create
        public ActionResult CreateActionCategory()
        {
            return View();
        }

        // POST: Sections/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateActionCategory([Bind(Include = "ActionCategoryName,Description")] ActionCategory actionCategory)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ActionCategories.Add(actionCategory);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("ActionCategoryList");
            }

            return View(actionCategory);
        }

        // GET: Sections/Edit/5
        public async Task<ActionResult> UpdateActionCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionCategory actionCategoryToBeUpdated = await _dbContext.ActionCategories.FindAsync(id);
            if (actionCategoryToBeUpdated == null)
            {
                return HttpNotFound();
            }
            return View(actionCategoryToBeUpdated);
        }

        // POST: Sections/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateActionCategory(int id, ActionCategory actionCategory)
        {
            if (ModelState.IsValid)
            {
                ActionCategory actionCategoryToBeUpdated = await _dbContext.ActionCategories.FindAsync(id);
                if (TryUpdateModel(actionCategoryToBeUpdated,"", new string[]{ "ActionCategoryName","Description" }))
                {
                    await _dbContext.SaveChangesAsync();
                }
                
                return RedirectToAction("ActionCategoryList");
            }
            return View(actionCategory);
        }

        // GET: Sections/Delete/5
        public async Task<ActionResult> DeleteActionCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionCategory actionCategoryToBeDeleted = await _dbContext.ActionCategories.FindAsync(id);
            if (actionCategoryToBeDeleted == null)
            {
                return HttpNotFound();
            }
            return View(actionCategoryToBeDeleted);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("DeleteActionCategory")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteActionCategoryConfirmed(int id)
        {
            ActionCategory actionCategoryToBeDeleted = await _dbContext.ActionCategories.FindAsync(id);
            if (actionCategoryToBeDeleted != null)
            {
                _dbContext.ActionCategories.Remove(actionCategoryToBeDeleted);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("ActionCategoryList");
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
