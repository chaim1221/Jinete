using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Jinete.Models;
using Jinete.ModelExtensions;
using Jinete.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jinete.Controllers
{
    public class NotebookController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> um;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Notebook/
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            List<NotebookViewModel> notebookList = new List<NotebookViewModel>();
            var notes = db.Notebooks.ToList();

            foreach(Notebook note in notes)
            {
                NotebookViewModel noteView = new NotebookViewModel();
                ApplicationUser _user = um.FindById(note.ApplicationUserId);
                noteView._notebook = note;
                noteView._username = _user.FirstName + " " + _user.LastName;
                notebookList.Add(noteView);
            }

            return View(notebookList.AsEnumerable());
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
            NotebookCreateModel model = new NotebookCreateModel();
            List<ApplicationUser> users = db.Users.ToList();
            string selectId = users.Where(x => x.Id == User.Identity.GetUserId()).Select(x => x.Id).First();
            IEnumerable<SelectListItem> selectList = users.AsEnumerable()
                .ToSelectListItems(selectId);
            model.Users = new SelectList(selectList, "Value", "Text", selectId);
            return View(model);
        }

        // POST: /Notebook/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(NotebookCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Notebook _notebook = (Notebook)model;
                db.Notebooks.Add(_notebook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
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
