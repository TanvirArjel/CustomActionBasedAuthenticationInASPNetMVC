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
    public class ActionCategoriesController : Controller
    {
        private UserDbContext db = new UserDbContext();

        // GET: Sections
        public async Task<ActionResult> Index()
        {
            return View(await db.ActionCategories.ToListAsync());
        }

        // GET: Sections/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionCategory section = await db.ActionCategories.FindAsync(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // GET: Sections/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sections/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ActionCategoryName,Description")] ActionCategory actionCategory)
        {
            if (ModelState.IsValid)
            {
                db.ActionCategories.Add(actionCategory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(actionCategory);
        }

        // GET: Sections/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionCategory actionCategory = await db.ActionCategories.FindAsync(id);
            if (actionCategory == null)
            {
                return HttpNotFound();
            }
            return View(actionCategory);
        }

        // POST: Sections/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SectionId,SectionName,Description")] ActionCategory actionCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(actionCategory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(actionCategory);
        }

        // GET: Sections/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActionCategory section = await db.ActionCategories.FindAsync(id);
            if (section == null)
            {
                return HttpNotFound();
            }
            return View(section);
        }

        // POST: Sections/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ActionCategory section = await db.ActionCategories.FindAsync(id);
            db.ActionCategories.Remove(section);
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
