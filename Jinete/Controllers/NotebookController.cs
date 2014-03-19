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

            // Validation failed, reassign the list without assigning anything
            List<ApplicationUser> users = db.Users.ToList();
            string selectId = users.Single(x => x.Id == model.ApplicationUserId).Id;
            IEnumerable<SelectListItem> selectList = users.AsEnumerable()
                .ToSelectListItems(selectId);
            model.Users = new SelectList(selectList, "Value", "Text", selectId);

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

            NotebookEditModel model = new NotebookEditModel();
            model._notebook = db.Notebooks.Find(id); ;
            if (model._notebook == null)
            {
                return HttpNotFound();
            }

            List<ApplicationUser> users = db.Users.ToList();

            // Grabbing the user here for the benefit of the "Details" view. We do not use the info here.
            string selectId = users.Single(x => x.Id == model._notebook.ApplicationUserId).Id;
            IEnumerable<SelectListItem> selectList = users.AsEnumerable()
                .ToSelectListItems(selectId);
            model.Users = new SelectList(selectList, "Value", "Text", selectId);

            if (model._notebook.CheckoutId != null)
            { 
                foreach (var clue in model._notebook.CheckoutId)
                {
                    model._checkouts.Add((CheckoutViewModel)db.Checkouts.Single(x => x.CheckoutId == clue));
                }
            }

            model._sale = model._notebook.SaleId == null ? null : db.Sales.Single(x => x.SaleId == model._notebook.SaleId);

            return View(model);
        }

        // POST: /Notebook/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(NotebookEditModel model)
        {
            if (ModelState.IsValid)
            {
                Notebook _notebook = model._notebook;
                Sale _sale = model._sale;

                if (_sale != null)
                {
                    _notebook.SaleId = _sale.SaleId;
                    db.Entry(_sale).State = EntityState.Modified;
                }
                db.Entry(_notebook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            List<ApplicationUser> users = db.Users.ToList();
            string selectId = users.Single(x => x.Id == model._notebook.ApplicationUserId).Id;
            IEnumerable<SelectListItem> selectList = users.AsEnumerable()
                .ToSelectListItems(selectId);
            model.Users = new SelectList(selectList, "Value", "Text", selectId);

            return View(model);
        }

        // GET: /Notebook/Delete/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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
