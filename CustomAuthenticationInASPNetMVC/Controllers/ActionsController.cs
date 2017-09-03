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
    public class ActionsController : Controller
    {
        private readonly UserDbContext db = new UserDbContext();

        // GET: SActions
        public async Task<ActionResult> Index()
        {
            var actions = db.ControllerActions.Include(s => s.ActionCategory);
            return View(await actions.ToListAsync());
        }

        // GET: SActions/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControllerAction sAction = await db.ControllerActions.FindAsync(id);
            if (sAction == null)
            {
                return HttpNotFound();
            }
            return View(sAction);
        }

        // GET: SActions/Create
        public ActionResult Create()
        {
            ViewBag.ActionCategoryId = new SelectList(db.ActionCategories, "ActionCategoryId", "ActionCategoryName");
            return View();
        }

        // POST: SActions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ActionId,SectionId,ActionName,Description")] ControllerAction controllerAction)
        {
            if (ModelState.IsValid)
            {
                db.ControllerActions.Add(controllerAction);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ActionCategoryId = new SelectList(db.ActionCategories, "ActionCategoryId", "ActionCategoryName", controllerAction.ActionCategoryId);
            return View(controllerAction);
        }

        // GET: SActions/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControllerAction controllerAction = await db.ControllerActions.FindAsync(id);
            if (controllerAction == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActionCategoryId = new SelectList(db.ActionCategories, "ActionCategoryId", "ActionCategoryName", controllerAction.ActionCategoryId);
            return View(controllerAction);
        }

        // POST: SActions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ActionId,SectionId,ActionName,Description")] ControllerAction controllerAction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(controllerAction).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ActionCategoryId = new SelectList(db.ActionCategories, "ActionCategoryId", "ActionCategoryName", controllerAction.ActionCategoryId);
            return View(controllerAction);
        }

        // GET: SActions/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ControllerAction sAction = await db.ControllerActions.FindAsync(id);
            if (sAction == null)
            {
                return HttpNotFound();
            }
            return View(sAction);
        }

        // POST: SActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ControllerAction controllerAction = await db.ControllerActions.FindAsync(id);
            db.ControllerActions.Remove(controllerAction);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
