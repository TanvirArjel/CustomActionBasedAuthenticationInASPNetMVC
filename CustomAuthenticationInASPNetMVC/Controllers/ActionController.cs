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
    public class ActionController : Controller
    {
        private readonly UserDbContext _dbContext = new UserDbContext();

        // GET: SActions
        public async Task<ActionResult> ControllerActionList()
        {
            List<ControllerAction> actionList = await _dbContext.ControllerActions.Include(s => s.ActionCategory).ToListAsync();
            return View(actionList);
        }

        // GET: SActions/Details/5
        public async Task<ActionResult> ControllerActionDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControllerAction controllerAction = await _dbContext.ControllerActions.FindAsync(id);
            if (controllerAction == null)
            {
                return HttpNotFound();
            }
            return View(controllerAction);
        }

        private async Task PopulateActionCategoryDropDownList(int? selectedActionCategory = null)
        {
            List<ActionCategory> actionCategories = await _dbContext.ActionCategories.ToListAsync();
            ViewBag.ActionCategoryId = new SelectList(actionCategories, "ActionCategoryId", "ActionCategoryName",selectedActionCategory);
        }
        // GET: SActions/Create
        public async Task<ActionResult> CreateControllerAction()
        {
            await PopulateActionCategoryDropDownList();
            return View();
        }

        // POST: SActions/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateControllerAction([Bind(Include = "ActionCategoryId,ActionName,Description")] ControllerAction controllerAction)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ControllerActions.Add(controllerAction);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("ControllerActionList");
            }

            await PopulateActionCategoryDropDownList(controllerAction.ActionCategoryId);
            return View(controllerAction);
        }

        // GET: SActions/Edit/5
        public async Task<ActionResult> UpdateControllerAction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControllerAction controllerActionToBeUpdated = await _dbContext.ControllerActions.FindAsync(id);
            if (controllerActionToBeUpdated == null)
            {
                return HttpNotFound();
            }
            await PopulateActionCategoryDropDownList(controllerActionToBeUpdated.ActionCategoryId);
            return View(controllerActionToBeUpdated);
        }

        // POST: SActions/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateControllerAction(int id,ControllerAction controllerAction)
        {
            if (ModelState.IsValid)
            {
                ControllerAction controllerActionToBeUpdated = await _dbContext.ControllerActions.FindAsync(id);
                if (TryUpdateModel(controllerActionToBeUpdated,"", new []{ "ActionCategoryId", "ActionName", "Description" }))
                {
                    await _dbContext.SaveChangesAsync();
                }
                
                return RedirectToAction("ControllerActionList");
            }
            await PopulateActionCategoryDropDownList(controllerAction.ActionCategoryId);
            return View(controllerAction);
        }

        // GET: SActions/Delete/5
        public async Task<ActionResult> DeleteControllerAction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControllerAction controllerActionToBeDeleted = await _dbContext.ControllerActions.FindAsync(id);
            if (controllerActionToBeDeleted == null)
            {
                return HttpNotFound();
            }
            return View(controllerActionToBeDeleted);
        }

        // POST: SActions/Delete/5
        [HttpPost, ActionName("DeleteControllerAction")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteControllerActionConfirmed(int id)
        {
            ControllerAction controllerActionToBeDeleted = await _dbContext.ControllerActions.FindAsync(id);
            if (controllerActionToBeDeleted != null)
            {
                _dbContext.ControllerActions.Remove(controllerActionToBeDeleted);
                await _dbContext.SaveChangesAsync();
            }
            return RedirectToAction("ControllerActionList");
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
