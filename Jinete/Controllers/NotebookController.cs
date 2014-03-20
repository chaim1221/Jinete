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

        // GET: /Notebook/ this is what the browser is doing
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            List<NotebookViewModel> notebookList = new List<NotebookViewModel>();
            var notes = db.Notebooks.ToList();

            foreach(Notebook note in notes)
            {
                NotebookViewModel noteView = new NotebookViewModel();
                ApplicationUser _user = note.ApplicationUser;
                noteView._notebook = note;
                noteView._username = _user.FirstName + " " + _user.LastName;
                noteView._sold = note.Sale ?? null;
                notebookList.Add(noteView);
            }

            return View(notebookList.AsEnumerable());
        }

        // GET: /Notebook/Checkout/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Checkout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new CheckoutCreateModel();

            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // POST: /Notebook/Checkout/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Checkout(CheckoutCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Checkout _checkout = new Checkout
                    {
                        ApplicationUser = um.FindById(model.ApplicationUserId),
                        dtCheckedOut = model.dtCheckedOut,
                        EquipmentId = model.EquipmentId,
                        EquipmentType = "Notebook"
                    };
                Notebook _notebook = db.Notebooks.Find(model.EquipmentId);
                //This looks right....
                _notebook.Checkouts.Add(_checkout);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Return(int? id)
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
            // I think I can, I think I can....
            Checkout checkout = notebook.Checkouts.Last();
            notebook.isCheckedOut = false;
            checkout.dtReturned = DateTime.Now;
            db.Entry(notebook).State = EntityState.Modified;
            db.Entry(checkout).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
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
            NotebookDetailsModel details = new NotebookDetailsModel();
            details._notebook = notebook;
            details._user = notebook.ApplicationUser;

            if (notebook.Checkouts != null)
            {
                details._checkouts = db.Checkouts
                    .Where(x => x.EquipmentType == "Notebook" && x.EquipmentId == notebook.NotebookId)
                    .Select(x => new CheckoutViewModel
                    {
                        dtCheckedOut = x.dtCheckedOut,
                        dtReturned = x.dtReturned,
                        Username = details._user.FirstName + " " + details._user.LastName
                    }).ToList();
            }

            details._sale = notebook.Sale == null ? null : db.Sales.Single(x => x.SaleId == notebook.Sale.SaleId);

            return View(details);
        }

        // GET: /Notebook/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            EquipmentCreateModel model = new EquipmentCreateModel();
            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);
            return View(model);
        }

        // POST: /Notebook/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(EquipmentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Notebook _notebook = new Notebook
                    {
                        ApplicationUser = um.FindById(model.ApplicationUserId),
                        EquipmentName = model.EquipmentName,
                        SerialNumber = model.SerialNumber,
                        PurchasePrice = model.PurchasePrice,
                        isCheckedOut = false
                    };
                db.Notebooks.Add(_notebook);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

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

            Notebook _notebook = db.Notebooks.Find(id);
            if (_notebook == null)
            {
                return HttpNotFound();
            }
            
            NotebookEditModel model = new NotebookEditModel(_notebook);
            
            string selectId = _notebook.ApplicationUser.Id;
            model.Users = FullNameUserList(db, selectId);
            
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
                Notebook _notebook = db.Notebooks.Find(model.NotebookId);

                _notebook.ApplicationUser = um.FindById(model.ApplicationUserId);
                _notebook.Discarded = model.Discarded;
                _notebook.EquipmentName = model.EquipmentName;
                _notebook.LostOrStolen = model.LostOrStolen;
                _notebook.PurchasePrice = model.PurchasePrice;
                _notebook.SerialNumber = model.SerialNumber;
                
                if (model.dtSold != null)
                {
                    Sale _sale = new Sale
                        {
                            dtSold = (DateTime)model.dtSold,
                            SalePrice = model.SalePrice ?? 0.0
                        };
                    db.Sales.Add(_sale);
                    db.SaveChanges();
                    _notebook.Sale = _sale;
                }

                db.Entry(_notebook).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

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

        public static SelectList FullNameUserList(ApplicationDbContext db, string id)
        {
            List<ApplicationUser> users = db.Users.ToList();
            IEnumerable<SelectListItem> selectList = users.AsEnumerable()
                .ToSelectUserList(id);
            return new SelectList(selectList, "Value", "Text", id);
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
