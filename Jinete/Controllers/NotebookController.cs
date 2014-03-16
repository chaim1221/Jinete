using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Jinete.Models;
using Jinete.ViewModels;

namespace Jinete.Controllers
{
    public class NotebookController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Notebook/
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            return View(db.Notebooks.ToList());
        }

        // GET: /Notebook/Details/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notebook notebook = db.Notebooks.Find(id);
            if (notebook == null)
            {
                return HttpNotFound();
            }
            return View(notebook);
        }

        // GET: /Notebook/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            NotebookViewModel notebookEditor = new NotebookViewModel();
            var userInfo = db.Users.Select(x => new {
                    firstName = x.FirstName,
                    lastName = x.LastName,
                    userId = x.Id
                })
                .ToList();

            notebookEditor._firstNames = userInfo.Select(x => x.firstName).ToList();
            notebookEditor._lastNames = userInfo.Select(x => x.lastName).ToList();
            notebookEditor._userIds = userInfo.Select(x => x.userId).ToList();

            return View(notebookEditor);
        }

        // POST: /Notebook/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create([Bind(Include = "NotebookId,ComputerName,PersonFirstName,PersonLastName,UserId,Phone,Address,City,State,Zip,Email,dtCheckedOut,dtReturned,checkedIn")] Notebook notebook)
        {
            if (ModelState.IsValid)
            {
                db.Notebooks.Add(notebook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(notebook);
        }

        // GET: /Notebook/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notebook notebook = db.Notebooks.Find(id);
            if (notebook == null)
            {
                return HttpNotFound();
            }
            return View(notebook);
        }

        // POST: /Notebook/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit([Bind(Include = "NotebookId,ComputerName,PersonFirstName,PersonLastName,Phone,Address,City,State,Zip,Email,dtCheckedOut,dtReturned,checkedIn")] Notebook notebook)
        {
            if (ModelState.IsValid)
            {
                db.Entry(notebook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notebook);
        }

        // GET: /Notebook/Delete/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notebook notebook = db.Notebooks.Find(id);
            if (notebook == null)
            {
                return HttpNotFound();
            }
            return View(notebook);
        }

        // POST: /Notebook/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            Notebook notebook = db.Notebooks.Find(id);
            db.Notebooks.Remove(notebook);
            db.SaveChanges();
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
