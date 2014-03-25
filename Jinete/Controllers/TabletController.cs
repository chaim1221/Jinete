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
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Jinete.Controllers
{
    public class TabletController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> um;

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            um = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: /Tablet/ this is what the browser is doing
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Index()
        {
            List<TabletViewModel> tabletList = new List<TabletViewModel>();
            var tabs = db.Tablets.ToList();

            foreach (Tablet tab in tabs)
            {
                TabletViewModel tabView = new TabletViewModel();
                ApplicationUser _user = tab.ApplicationUser;
                tabView._tablet = tab;
                tabView._username = _user.FirstName + " " + _user.LastName;
                tabView._lastcheckout = tab.Checkouts
                    .OrderBy(x => x.dtCheckedOut)
                    .LastOrDefault() == null ? null :
                        tab.Checkouts.OrderBy(x => x.dtCheckedOut)
                        .Select(x => new CheckoutViewModel
                        {
                            dtCheckedOut = x.dtCheckedOut,
                            dtReturned = x.dtReturned,
                            Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                        }).LastOrDefault();
                tabView._sold = tab.Sale ?? null;
                tabletList.Add(tabView);
            }

            return View(tabletList.AsEnumerable());
        }

        // GET: /Tablet/Checkout/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Checkout(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new CheckoutCreateModel();
            model.EquipmentId = (int)id;
            model.dtCheckedOut = DateTime.Now;

            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // POST: /Tablet/Checkout/5
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
                        dtCheckedOut = model.dtCheckedOut//,
                        // Early attempt to create a *-1 rel.
                        //EquipmentId = model.EquipmentId,
                        //EquipmentType = "Tablet"
                    };
                Tablet _tablet = db.Tablets.Find(model.EquipmentId);
                ApplicationUser _user = _tablet.ApplicationUser;
                _tablet.isCheckedOut = true;
                _tablet.Checkouts.Add(_checkout);
                _tablet.ApplicationUser = _user; // No fucking clue if/why this is necessary.

                db.Entry(_tablet).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    GetDbErrorState(dbEx);
                }

                return RedirectToAction("Index");
            }

            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tablet tablet = db.Tablets.Find(id);
            if (tablet == null)
            {
                return HttpNotFound();
            }
            // I think I can, I think I can....
            Checkout checkout = tablet.Checkouts.OrderBy(x => x.dtCheckedOut).Last();
            ApplicationUser TabletUser = tablet.ApplicationUser;
            ApplicationUser CheckoutUser = checkout.ApplicationUser;
            tablet.isCheckedOut = false;
            checkout.dtReturned = DateTime.Now;
            checkout.ApplicationUser = CheckoutUser;
            tablet.ApplicationUser = TabletUser; //once again the insanity

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                GetDbErrorState(dbEx);
            }

            return RedirectToAction("Index");
        }

        // GET: /Tablet/Details/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tablet tablet = db.Tablets.Find(id);
            if (tablet == null)
            {
                return HttpNotFound();
            }
            TabletViewModel details = new TabletViewModel();
            details._tablet = tablet;

            details._lastcheckout = tablet.Checkouts.Any() ? tablet.Checkouts.OrderByDescending(x => x.dtCheckedOut).Select(x => new CheckoutViewModel
                {
                    dtCheckedOut = x.dtCheckedOut,
                    dtReturned = x.dtReturned,
                    Username = x.ApplicationUser.FirstName + " " + x.ApplicationUser.LastName
                }).First() : null;

            details._sold = tablet.Sale == null ? null : tablet.Sale;

            return View(details);
        }

        // GET: /Tablet/Create
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create()
        {
            EquipmentCreateModel model = new EquipmentCreateModel();
            string selectId = User.Identity.GetUserId();
            model.Users = FullNameUserList(db, selectId);
            return View(model);
        }

        // POST: /Tablet/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Create(EquipmentCreateModel model)
        {
            if (ModelState.IsValid)
            {
                Tablet _tablet = new Tablet
                    {
                        ApplicationUser = um.FindById(model.ApplicationUserId),
                        EquipmentName = model.EquipmentName,
                        SerialNumber = model.SerialNumber,
                        PurchasePrice = model.PurchasePrice,
                        isCheckedOut = false
                    };
                db.Tablets.Add(_tablet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /Tablet/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Tablet _tablet = db.Tablets.Find(id);
            if (_tablet == null)
            {
                return HttpNotFound();
            }

            TabletEditModel model = new TabletEditModel(_tablet);

            string selectId = _tablet.ApplicationUser.Id;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // POST: /Tablet/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public ActionResult Edit(TabletEditModel model)
        {
            if (ModelState.IsValid)
            {
                Tablet _tablet = db.Tablets.Find(model.TabletId);

                _tablet.ApplicationUser = um.FindById(model.ApplicationUserId);
                _tablet.Discarded = model.Discarded;
                _tablet.EquipmentName = model.EquipmentName;
                _tablet.LostOrStolen = model.LostOrStolen;
                _tablet.PurchasePrice = model.PurchasePrice;
                _tablet.SerialNumber = model.SerialNumber;

                if (model.dtSold != null)
                {
                    Sale _sale = new Sale
                        {
                            dtSold = (DateTime)model.dtSold,
                            SalePrice = model.SalePrice ?? 0.0
                        };
                    db.Sales.Add(_sale);
                    db.SaveChanges();
                    _tablet.Sale = _sale;
                }

                db.Entry(_tablet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Validation failed, reassign the list without assigning anything
            string selectId = model.ApplicationUserId;
            model.Users = FullNameUserList(db, selectId);

            return View(model);
        }

        // GET: /Tablet/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tablet tablet = db.Tablets.Find(id);
            if (tablet == null)
            {
                return HttpNotFound();
            }
            return View(tablet);
        }

        // POST: /Tablet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Tablet tablet = db.Tablets.Find(id);
            db.Tablets.Remove(tablet);
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

        public static void GetDbErrorState(DbEntityValidationException dbEx)
        {
            foreach (var validationErrors in dbEx.EntityValidationErrors)
            {
                foreach (var validationError in validationErrors.ValidationErrors)
                {
                    Trace.TraceInformation("Property: {0} Error: {1} Entity: {2}", validationError.PropertyName, validationError.ErrorMessage, validationErrors.Entry.Entity.GetType().FullName);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                um.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}